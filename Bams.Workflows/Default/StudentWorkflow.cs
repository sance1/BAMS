using Bams.Workflows.Enums;
using Bams.Workflows.Models;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using EightElements.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows.Default
{
    public class StudentWorkflow : IStudentWorkflow
    {
        private IUnitOfWork _uow;
        private IChangelogService _changelog;
        private readonly ILogger<StudentWorkflow> _logger;


        public StudentWorkflow(
            IUnitOfWork unitOfWork,
            IChangelogService changelogService,
            ILogger<StudentWorkflow> logger)
        {
            _uow = unitOfWork;
            _changelog = changelogService;
            _logger = logger;
        }

        public async Task<WorkflowResult> CreateStudent(int userId, StudentDto dto)
        {
            try
            {
                var admin = await _uow.AccountRepository.GetByIdAsync(userId);
                var school = await _uow.SchoolRepository.GetByIdAsync(admin.SchoolId.Value);

                var student = new UserAccount
                {
                    Uid = await _uow.UserAccountRepository.GenerateUid(),
                    ProjectId = school.ProjectId,
                    DistrictId = school.DistrictId,
                    SchoolId = school.Id,
                    Class = dto.Class,
                    Name = dto.Name,
                    UserName = dto.Username,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    ActivationStatus = 0,
                    CreateDate = DateTime.Now,
                    CreatedBy = userId
                };
                await _uow.UserAccountRepository.AddAsync(student);

                return WorkflowResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }

        
        public async Task<WorkflowResult> UpdateStudent(int userId, StudentDto dto)
        {
            try
            {
                var student = await _uow.UserAccountRepository.GetByUidAsync(dto.Uid);
                if (student == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                var school = await _uow.SchoolRepository.GetByIdAsync(student.SchoolId);
                
                string oldValue = JsonConvert.SerializeObject(student);
                student.Class = dto.Class;
                student.Name = dto.Name;
                student.UserName = dto.Username;
                student.PhoneNumber = dto.PhoneNumber;
                student.Email = dto.Email;
                student.SchoolId = school.Id;
                student.DistrictId = school.DistrictId;
                student.ProjectId = school.ProjectId;
                await _uow.SchoolRepository.UpdateAsync(school);

                string newValue = JsonConvert.SerializeObject(student);
                await _changelog.Log("Student", student.Id, userId, oldValue, newValue);

                return WorkflowResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }


        public async Task<WorkflowResult> DeleteStudent(int userId, long uid)
        {
            try
            {
                var student = await _uow.UserAccountRepository.GetByUidAsync(uid);

                if (student == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                bool hasPermission = await HasDeletePermission(userId, student);
                if (!hasPermission)
                {
                    return WorkflowResult.AccessViolation;
                }

                if (student.ActivationStatus > 0)
                {
                    return WorkflowResult.ActionProhibited;
                }
                
                student.DeleteDate = DateTime.Now;
                student.DeletedBy = userId;
                await _uow.UserAccountRepository.UpdateAsync(student);

                return WorkflowResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }

        }


        private async Task<bool> HasDeletePermission(int userId, UserAccount student)
        {
            var user = await _uow.AccountRepository.GetByIdAsync(userId);
            if (user.SchoolId == student.SchoolId) return true;
            return false;
        }

        public async Task<WorkflowResult> ActivateAccount(int userId, long studentUid)
        {
            try
            {
                var user = await _uow.UserAccountRepository.GetByIdAsync(userId);

                var student = await _uow.UserAccountRepository.GetByUidAsync(studentUid);
                if (student == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                if (user.SchoolId != student.SchoolId)
                {
                    return WorkflowResult.AccessViolation;
                }

                var activationCode =
                    await _uow.activationCodeRepository.GetActivationCode(user.SchoolId);

                if (activationCode == null)
                {
                    return WorkflowResult.ActivationCodeNotFound;
                    //todo: send email notification
                }

                //call Educa's API to activate account

                //if activation is successful, update 
                activationCode.Status = ActivationCodeStatus.Redeemed;
                activationCode.RedeemDate = DateTime.Now;
                activationCode.RedeemedBy = user.Id;
                activationCode.RedeemMethod = RedeemMethods.Bams;
                await _uow.activationCodeRepository.UpdateAsync(activationCode);

                return WorkflowResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }

        public async Task<WorkflowResult> DismissClass(int userId, string className)
        {
            var user = await _uow.UserAccountRepository.GetByIdAsync(userId);
            try
            {
                var students = await _uow.UserAccountRepository.GetAsync(
                    predicate: s =>
                        s.SchoolId == user.SchoolId &&
                        s.Class == className &&
                        s.DeleteDate == null);

                foreach (var student in students)
                {
                    student.DeleteDate = DateTime.Now;
                    student.DeletedBy = userId;
                    await _uow.UserAccountRepository.UpdateAsync(student);
                }

                return WorkflowResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }

        }
    }
}
