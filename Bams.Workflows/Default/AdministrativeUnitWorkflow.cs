using Bams.Workflows.Enums;
using Bams.Workflows.Models;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using EightElements.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bams.Workflows.Default
{
    public class AdministrativeUnitWorkflow : IAdministrativeUnitWorkflow
    {
        private IUnitOfWork _uow;
        private IChangelogService _changelog;
        private readonly ILogger<AdministrativeUnitWorkflow> _logger;


        public AdministrativeUnitWorkflow(
            IUnitOfWork unitOfWork,
            IChangelogService changelogService,
            ILogger<AdministrativeUnitWorkflow> logger)
        {
            _uow = unitOfWork;
            _changelog = changelogService;
            _logger = logger;
        }

        public async Task<WorkflowResult> Create(int userId, AdministrativeUnitDto dto)
        {
            try
            {
                var parent = await _uow.AdministrativeUnitRepository.GetByUidAsync(dto.ParentUid);
                int administrativeLevel = 1;
                string path = null;
                if (parent != null)
                {
                    administrativeLevel = parent.AdministrativeLevel + 1;
                    path = $"{parent.Path}/{parent.Id}";
                }
                var unit = new AdministrativeUnit
                {
                    Uid = await _uow.SchoolRepository.GenerateUid(),
                    ProjectId = await _uow.ProjectRepository.GetIdByUid(dto.ProjectUid),
                    Name = dto.Name,
                    PIC = dto.PIC,
                    AdministrativeLevel = administrativeLevel,
                    ParentId = parent?.Id,
                    Path = null,
                    Remarks = dto.Remarks,
                    CreateDate = DateTime.Now,
                    CreatedBy = userId
                };
                await _uow.AdministrativeUnitRepository.AddAsync(unit);

                unit.Path = $"{parent?.Path}/{unit.Id}";
                await _uow.AdministrativeUnitRepository.UpdateAsync(unit);

                return WorkflowResult.Success;

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }

        public async Task<WorkflowResult> Delete(int userId, long uid)
        {
            try
            {
                var unit = await _uow.AdministrativeUnitRepository.GetByUidAsync(uid);
                if (unit == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                var subUnits = await _uow.AdministrativeUnitRepository.CountAsync(u =>
                    EF.Functions.Like(u.Path, $"{unit.Path}%") &&
                    u.Id != unit.Id);
                if (subUnits > 0)
                {
                    return WorkflowResult.ActionProhibited;
                }

                var schoolCount = await _uow.SchoolRepository.CountAsync(s =>
                    s.DistrictId == unit.Id);
                if (schoolCount > 0)
                {
                    return WorkflowResult.ActionProhibited;
                }

                unit.DeleteDate = DateTime.Now;
                unit.DeletedBy = userId;
                await _uow.AdministrativeUnitRepository.UpdateAsync(unit);

                return WorkflowResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }

        public async Task<WorkflowResult> Update(int userId, AdministrativeUnitDto dto)
        {
            try
            {
                var unit = await _uow.AdministrativeUnitRepository.GetByUidAsync(dto.Uid);
                if (unit == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                //todo: tidy up and optimize the following codes:
                int oldParentId = unit.ParentId.HasValue ? unit.ParentId.Value : 0;
                var oldParent = await _uow.AdministrativeUnitRepository.GetByIdAsync(oldParentId);
                var newParent = await _uow.AdministrativeUnitRepository.GetByUidAsync(dto.ParentUid);
                int newParentId = newParent != null ? newParent.Id : 0;

                if (newParentId != oldParentId)
                {
                    string oldPath = unit.Path;
                    string newPath = $"{newParent?.Path}/{unit.Id}";
                    //await _uow.AdministrativeUnitRepository.GetPathByUid(dto.ParentUid);
                    //var newPath = $"{newParent.Path}/{newParent.Id}";

                    await UpdateChilds(unit.Id, oldPath, newPath);
                }

                int administrativeLevel = 1;
                string path = $"/{unit.Id}";
                if (newParent != null)
                {
                    administrativeLevel = newParent.AdministrativeLevel + 1;
                    path = $"{newParent.Path}/{unit.Id}";
                }

                string oldValue = JsonConvert.SerializeObject(unit);

                unit.ProjectId = await _uow.ProjectRepository.GetIdByUid(dto.ProjectUid);
                unit.Name = dto.Name;
                unit.PIC = dto.PIC;
                unit.AdministrativeLevel = administrativeLevel;
                unit.ParentId = newParent?.Id;
                unit.Path = path;
                unit.Remarks = dto.Remarks;
                _uow.AdministrativeUnitRepository.Update(unit);
                await _uow.SaveAsync();


                string newValue = JsonConvert.SerializeObject(unit);
                await _changelog.Log("AdministrativeUnit", unit.Id, userId, oldValue, newValue);



                return WorkflowResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }


        private async Task UpdateChilds(int parentId, string oldPath, string newPath)
        {
            var childUnits = await _uow.AdministrativeUnitRepository.GetAsync(predicate: u =>
                        EF.Functions.Like(u.Path, $"{oldPath}%") &&
                        u.Id != parentId); // exclude parent unit

            foreach (var item in childUnits)
            {
                item.Path = Regex.Replace(item.Path, $"^{oldPath}", newPath);
                item.AdministrativeLevel = item.Path.Count(s => s == '/');
                _uow.AdministrativeUnitRepository.Update(item);
            }
        }
    }
}
