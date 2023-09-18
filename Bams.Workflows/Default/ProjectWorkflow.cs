using BAMS.Data.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BAMS.Data.Models;
using BAMS.Workflows.Models;
using EightElements.Services;
using Newtonsoft.Json;
using Bams.Workflows.Enums;

namespace Bams.Workflows.Default
{
    public class ProjectWorkflow : IProjectWorkflow
    {
        private IUnitOfWork _uow;
        private IChangelogService _changelog;
        private readonly ILogger<ProjectWorkflow> _logger;

        public ProjectWorkflow(
            IUnitOfWork unitOfWork,
            IChangelogService changelogService,
            ILogger<ProjectWorkflow> logger)
        {
            _uow = unitOfWork;
            _changelog = changelogService;
            _logger = logger;
        }
                

        public async Task<WorkflowResult> CreateProject(
            int userId, ProjectDto dto)
        {
            try
            {
                var project = new Project
                {
                    Uid = await _uow.ProjectRepository.GenerateUid(),
                    AppId = await _uow.ApplicationRepository.GetIdByUid(dto.AppUid),
                    Name = dto.Name,
                    PartnerName = dto.PartnerName,
                    ContactPerson = dto.ContactPerson,
                    PartnerPIC = dto.PartnerPIC,
                    Remarks = dto.Remarks,
                    CreateDate = DateTime.Now,
                    CreatedBy = userId,
                    CountryId = dto.CountryId,
                    ProvinceId = dto.ProvinceId
                };
                await _uow.ProjectRepository.AddAsync(project);

                return WorkflowResult.Success;

            } catch(Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }


        public async Task<WorkflowResult> UpdateProject(int userId, ProjectDto dto)
        {
            try
            {
                var project = await _uow.ProjectRepository.GetByUidAsync(dto.Uid);
                if (project == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                string oldValue = JsonConvert.SerializeObject(project);
                project.AppId = await _uow.ApplicationRepository.GetIdByUid(dto.AppUid);
                project.Name = dto.Name;
                project.PartnerName = dto.PartnerName;
                project.ContactPerson = dto.ContactPerson;
                project.PartnerPIC = dto.PartnerPIC;
                project.Remarks = dto.Remarks;
                project.CountryId = dto.CountryId;
                project.ProvinceId = dto.ProvinceId;

                await _uow.ProjectRepository.UpdateAsync(project);
                string newValue = JsonConvert.SerializeObject(project);
                await _changelog.Log("Project", project.Id, userId, oldValue, newValue);

                return WorkflowResult.Success;

            } catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }

        public async Task<WorkflowResult> DeleteProject(int userId, long uid)
        {
            try
            {
                var project = await _uow.ProjectRepository.GetByUidAsync(uid);
                if (project == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                var contract = await _uow.ContractRepository.CountAsync(d => d.ProjectId == project.Id);
                if (contract > 0)
                {
                    return WorkflowResult.ActionProhibited;
                }

                project.DeleteDate = DateTime.Now;
                project.DeletedBy = userId;
                await _uow.ProjectRepository.UpdateAsync(project);

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
