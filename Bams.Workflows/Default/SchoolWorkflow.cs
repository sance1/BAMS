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
    public class SchoolWorkflow : ISchoolWorkflow
    {
        private IUnitOfWork _uow;
        private IChangelogService _changelog;
        private readonly ILogger<ContractWorkflow> _logger;


        public SchoolWorkflow(
           IUnitOfWork unitOfWork,
           IChangelogService changelogService,
           ILogger<ContractWorkflow> logger)
        {
            _uow = unitOfWork;
            _changelog = changelogService;
            _logger = logger;
        }


        public async Task<WorkflowResult> CreateSchool(int userId, SchoolDto dto)
        {
            try
            {
                //var district = await _uow.DistrictRepository.GetByUidAsync(dto.DistrictUid);
                var admUnit = await _uow.AdministrativeUnitRepository.GetByUidAsync(dto.AdmUnitUid);

                var school = new School
                {
                    Uid = await _uow.SchoolRepository.GenerateUid(),
                    //DistrictId = district.Id,
                    AdministrativeUnitId = admUnit.Id,
                    ProjectId = admUnit.ProjectId,
                    Name = dto.Name,
                    Address = dto.Address,
                    PIC = dto.PIC,
                    Students = dto.Students,                    
                    Remarks = dto.Remarks,
                    CreateDate = DateTime.Now,
                    CreatedBy = userId
                };
                await _uow.SchoolRepository.AddAsync(school);

                return WorkflowResult.Success;

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }


        public async Task<WorkflowResult> UpdateSchool(int userId, SchoolDto dto)
        {
            try
            {
                var school = await _uow.SchoolRepository.GetByUidAsync(dto.Uid);
                if (school == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                //var district = await _uow.DistrictRepository.GetByUidAsync(dto.DistrictUid);
                var admUnit = await _uow.AdministrativeUnitRepository.GetByUidAsync(dto.AdmUnitUid);

                string oldValue = JsonConvert.SerializeObject(school);                
                school.Name = dto.Name;
                school.Address = dto.Address;
                school.PIC = dto.PIC;
                school.Students = dto.Students;
                school.Remarks = dto.Remarks;
                //school.DistrictId = district.Id;
                school.AdministrativeUnitId = admUnit.Id;
                school.ProjectId = admUnit.ProjectId;
                await _uow.SchoolRepository.UpdateAsync(school);

                string newValue = JsonConvert.SerializeObject(school);
                await _changelog.Log("School", school.Id, userId, oldValue, newValue);

                return WorkflowResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }


        public async Task<WorkflowResult> DeleteSchool(int userId, long uid)
        {
            try
            {
                var school = await _uow.SchoolRepository.GetByUidAsync(uid);
                if (school == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                bool hasPermission = await HasDeletePermission(userId, school);
                if (!hasPermission)
                {
                    return WorkflowResult.AccessViolation;
                }

                int allocatedCodes = await _uow.activationCodeRepository.CountAsync(a =>
                    a.SchoolId == school.Id);
                if (allocatedCodes > 0)
                {
                    return WorkflowResult.ActionProhibited;
                }

                int students = await _uow.AccountRepository.CountAsync(s => 
                    s.SchoolId == school.Id);
                if (students > 0)
                {
                    return WorkflowResult.ActionProhibited;
                }

                school.DeleteDate = DateTime.Now;
                school.DeletedBy = userId;
                await _uow.SchoolRepository.UpdateAsync(school);

                return WorkflowResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }

        

        private async Task<bool> HasDeletePermission(int userId, School school)
        {
            var user = await _uow.AccountRepository.GetByIdAsync(userId);

            if (user.ProjectId == 0 || // super admin
                (user.ProjectId > 0 && user.ProjectId == school.ProjectId) || // project admin
                (user.ProjectId == null && user.DistrictId == school.DistrictId))  // district admin
            {
                return true;
            }

            return false;
        }
    }
}
