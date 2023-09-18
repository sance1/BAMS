using Bams.Workflows;
using Bams.Workflows.Enums;
using Bams.Workflows.InputValidators;
using Bams.Workflows.Models;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Helpers;
using BAMS.Models;
using EightElements.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;

namespace BAMS.Controllers
{
    public class AdministrativeUnitController : BaseController
    {
        private IAdministrativeUnitWorkflow _workflow;
        private IAdministrativeUnitValidator _validator;
        private IUnitOfWork _uow;
        private ITextService _textService;
        private IMapper _mapper { get; set; }

        public AdministrativeUnitController(
            IAdministrativeUnitWorkflow workflow,
            IAdministrativeUnitValidator validator,
            IUnitOfWork unitOfWork,
            ITextService textService,
            IUnitOfWork uow)
            : base(unitOfWork, textService)
        {
            _workflow = workflow;
            _validator = validator;
            _uow = uow;
            _textService = textService;

            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<AdministrativeUnitDto, AdministrativeUnit>();
                    cfg.CreateMap<AdministrativeUnit, AdministrativeUnitDto>();
                });

                _mapper = config.CreateMapper();
            }
        }

        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Read)]
        public async Task<IActionResult> Index()
        {
            ViewData["ProjectList"] = await GetProjectList();
            ViewData["ProjectId"] = ProjectId;
            ViewData["RoleId"] = RoleId;
            ViewData["Projects"] = await _uow.ProjectRepository.GetAsync(
                predicate: p => p.DeleteDate == null);

            return View();
        }


        [Route("get-administrative-unit/{uid}")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Read)]
        public async Task<IActionResult> Get(long uid)
        {
            var unit = await _uow.AdministrativeUnitRepository.GetByUidAsync(uid);
            if (unit == null)
            {
                return Failed(GetText("Administrative_popup_txt_data_not_found"));
            }

            int parentId = unit.ParentId.HasValue ? unit.ParentId.Value : 0;
            var dto = new
            {
                uid = unit.Uid,
                projectUid = await _uow.ProjectRepository.GetUidById(unit.ProjectId),
                parentUid = await _uow.AdministrativeUnitRepository.GetUidById(parentId),
                name = unit.Name,
                picName = unit.PIC,
                remarks = unit.Remarks
            };

            return Success("administrativeUnit", JObject.FromObject(dto));
        }

        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetList([FromForm] DtParameters dtParameters)
        {
            var predicate = PredicateBuilder.True<AdministrativeUnit>();

            if (ProjectId > 0)
            {
                predicate = predicate.And(a => a.ProjectId == ProjectId);
            }

            foreach (var column in dtParameters.Columns)
            {
                string searchValue = column.Search.Value;
                if (string.IsNullOrEmpty(searchValue)) continue;

                switch (column.Name)
                {
                    case "ProjectName":
                        predicate = predicate.And(a => a.Project.Name.Contains(searchValue));
                        break;
                    case "Name":
                        predicate = predicate.And(a => a.Name.Contains(searchValue));
                        break;
                    case "PIC":
                        predicate = predicate.And(a => a.PIC.Contains(searchValue));
                        break;
                    case "Remarks":
                        predicate = predicate.And(a => a.Remarks.Contains(searchValue));
                        break;
                }
            }

            predicate = predicate.And(a => a.DeleteDate == null);

            var sortColumn = dtParameters.Columns[dtParameters.Order[0].Column].Name;
            Expression<Func<AdministrativeUnit, dynamic>> sortFunc = a => a.Id;

            switch (sortColumn)
            {
                case "ProjectName":
                    sortFunc = a => a.Project.Name;
                    break;
                case "Name":
                    sortFunc = a => a.Name;
                    break;
                case "PIC":
                    sortFunc = a => a.PIC;
                    break;
                case "Remarks":
                    sortFunc = a => a.Remarks;
                    break;
            }

            Func<IQueryable<AdministrativeUnit>, IOrderedQueryable<AdministrativeUnit>> orderBy =
                x => x.OrderBy(sortFunc);
            if (dtParameters.Order[0].Dir == DtOrderDir.Desc)
            {
                orderBy = x => x.OrderByDescending(sortFunc);
            }

            var list = await _uow.AdministrativeUnitRepository.GetAsync(
                predicate: predicate,
                selector: u =>
                    new AdministrativeUnitModel
                    {
                        Uid = u.Uid,
                        ProjectName = u.Project.Name,
                        Name = u.Name,
                        PIC = u.PIC,
                        Remarks = u.Remarks,
                        Schools = u.Schools.Count,
                        Students = u.Schools.Sum(a => a.Students),
                        ActivationCodes = u.ActivationCodes.Count,
                        ActivationCodesActive = u.ActivationCodes.Where(a => a.RedeemDate != null).Count(),
                    },
                orderBy: orderBy,
                usePaging: true,
                pageSize: dtParameters.Length,
                skip: dtParameters.Start,
                includeProperties: "ActivationCodes,Schools"
            );

            int recordsFiltered = await _uow.AdministrativeUnitRepository.CountAsync(predicate);
            int recordsTotal = await _uow.AdministrativeUnitRepository.CountAsync(a => true);

            var result = new DtResult<AdministrativeUnitModel>
            {
                Data = list,
                Draw = dtParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            var json = JsonConvert.SerializeObject(result);

            return Content(json, "application/json");
        }


        [Route("/AdministrativeUnit/GetByProject/{projectUid}")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetByProject(long projectUid)
        {
            var projectId = await _uow.ProjectRepository.GetIdByUid(projectUid);
            var list = await _uow.AdministrativeUnitRepository.GetAsync(
                predicate: u => u.ProjectId == projectId && u.DeleteDate == null,
                selector: u =>
                    new
                    {
                        uid = u.Uid,
                        name = u.Name
                    });

            return Success("administrativeUnits", JArray.FromObject(list));
        }


        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Create)]
        public async Task<IActionResult> Create([FromBody] AdministrativeUnitDto dto)
        {
            var validationResults = await _validator.ValidateCreate(dto);
            if (validationResults.Count > 0)
            {
                string message = string.Join("<br/>", validationResults);
                return Failed(message);
            }

            var result = await _workflow.Create(UserId, dto);
            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                default:
                    return Failed(GetText("Administrative_popup_unknow_error"));
            }
        }

        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Create)]
        public async Task<IActionResult> Update([FromBody] AdministrativeUnitDto dto)
        {
            var validationResults = await _validator.ValidateUpdate(dto);
            if (validationResults.Count > 0)
            {
                return Failed(string.Join("<br/>", validationResults));
            }

            var result = await _workflow.Update(UserId, dto);
            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:
                    return Failed(GetText("Administrative_popup_txt_data_not_found"));                
                default:
                    return Error(GetText("Administrative_popup_unknow_error"));
            }
        }

        [Route("delete-administrative-unit/{uid}")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Delete)]
        public async Task<IActionResult> Delete(long uid)
        {
            int userId = 16;
            var result = await _workflow.Delete(userId, uid);
            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:
                    return Failed(GetText("Administrative_popup_txt_data_not_found"));
                case WorkflowResult.ActionProhibited:
                    return Failed("Administrative_popup_cannot_be_delete_subdivision");
                default:
                    return Failed(GetText("Administrative_popup_unknow_error"));
            }
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile(string rawXls, int projectId = 0)
        {
            if (projectId < 1) projectId = ProjectId;
            var data = JsonConvert.DeserializeObject<List<UploadAdmUnitDTO>>(rawXls);
            data = data.GroupBy(a => a.AdministrativeUnitName).Select(a => a.First()).ToList();
            var admUnitName = data.Select(a => a.AdministrativeUnitName).ToList();

            var validationResults = new List<string>();
            var uploadedAdmUnits = new List<AdministrativeUnit>();
            var existedAdmUnit = await _uow.AdministrativeUnitRepository
                .GetAsync(predicate: a => a.ProjectId == projectId && admUnitName.Contains(a.Name));

            if (projectId < 1)
            {
                validationResults.Add(GetText("District_popup_txt_project_id_null"));
            }

            foreach (var upload in data)
            {
                if (string.IsNullOrEmpty(upload.AdministrativeUnitName) ||
                    string.IsNullOrEmpty(upload.PIC))
                {
                    validationResults.Add(GetText("District_popup_txt_incorrect_format"));
                }

                var admUnit = existedAdmUnit.Where(a => a.Name.ToLower() == upload.AdministrativeUnitName.ToLower())
                    .FirstOrDefault();

                if (admUnit == null)
                {
                    admUnit = new AdministrativeUnit();
                    admUnit.Name = upload.AdministrativeUnitName;
                    admUnit.PIC = upload.PIC;
                    admUnit.Uid = await _uow.AdministrativeUnitRepository.GenerateUid();
                    admUnit.ProjectId = projectId;
                    admUnit.CreateDate = DateTime.Now;
                    admUnit.CreatedBy = UserId;
                    uploadedAdmUnits.Add(admUnit);
                }
                else
                {
                    validationResults.Add(GetText("District_popup_txt_district_exists"));
                }
            }

            validationResults = validationResults.Distinct().ToList();

            if (validationResults.Count > 0)
            {
                var errorJson =
                    JsonConvert.SerializeObject(new {message = string.Join("\n", validationResults)});
                return Content(errorJson, "application/json");
            }

            await _uow.AdministrativeUnitRepository.AddRangeAsync(uploadedAdmUnits);

            var json = JsonConvert.SerializeObject(new {message = "success", status = 0});
            return Content(json, "application/json");
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAdministrativeUnit(string rawXls, int projectId = 0, int districtId = 0,
            int admUnitId = 0)
        {
            if (projectId < 1) projectId = ProjectId;

            var data = JsonConvert.DeserializeObject<List<UploadAdministrativeUnitDto>>(rawXls);
            var duplicateData = data.GroupBy(a => a.AdministrativeUnitName).Where(a => a.Count() > 1).Select(a => a.Key)
                .ToList();

            data = data.GroupBy(a => a.AdministrativeUnitName).Select(a => a.First()).ToList();
            var administrativeUnitNames = data.Select(a => a.AdministrativeUnitName).ToList();
            var parentAdministrativeUnitNames = data.Select(a => a.ParentAdministrativeUnitName).ToList();

            var validationResults = new List<string>();

            if (duplicateData.Count > 0)
            {
                var dupSchool = string.Join(", ", duplicateData);
                validationResults.Add($"Duplicated School Name found [{dupSchool}]");
            }

            if (projectId < 1)
            {
                validationResults.Add(GetText("School_popup_txt_project_id_null"));
            }

            var uploadAdministrativeUnits = new List<AdministrativeUnit>();
            var uploadAdministrativeUnitDtos = new List<AdministrativeUnitDto>();
            var existedAdministrativeUnit = await _uow.AdministrativeUnitRepository
                .GetAsync(predicate: a => a.ProjectId == projectId && administrativeUnitNames.Contains(a.Name));
            var parentExistedAdministrativeUnit = await _uow.AdministrativeUnitRepository
                .GetAsync(predicate: a => a.ProjectId == projectId && parentAdministrativeUnitNames.Contains(a.Name));

            var rowNum = 1;
            foreach (var upload in data)
            {
                if (string.IsNullOrEmpty(upload.AdministrativeUnitName) ||
                    string.IsNullOrEmpty(upload.PIC))
                {
                    validationResults.Add(
                        GetText("AdministrativeUnit_popup_txt_incorect_format_upload") + $" [row number {rowNum}]");
                }

                if (upload.AdministrativeUnitName.Length < 6)
                {
                    validationResults.Add(GetText("AdministrativeUnit_popup_txt_invalid_name") +
                                          $" [row number {rowNum}]");
                }

                var administrativeUnit = existedAdministrativeUnit
                    .Where(a => a.Name.ToLower() == upload.AdministrativeUnitName.ToLower()).FirstOrDefault();

                if (administrativeUnit == null)
                {
                    administrativeUnit = new AdministrativeUnit();
                    administrativeUnit.Name = upload.AdministrativeUnitName;
                    administrativeUnit.PIC = upload.PIC;
                    administrativeUnit.Remarks = upload.Remakrs;
                    administrativeUnit.Uid = await _uow.SchoolRepository.GenerateUid();
                    administrativeUnit.ProjectId = projectId;
                    administrativeUnit.CreateDate = DateTime.Now;
                    administrativeUnit.CreatedBy = UserId;

                    var dto = _mapper.Map<AdministrativeUnitDto>(administrativeUnit);
                    var parentAdministrative =
                        parentExistedAdministrativeUnit
                            .Where(a => a.Name.ToLower() == upload.ParentAdministrativeUnitName.ToLower())
                            .FirstOrDefault();
                    if (parentAdministrative != null)
                    {
                        dto.ParentUid = parentAdministrative.Uid;
                    }

                    if (dto.Remarks == null)
                    {
                        dto.Remarks = "";
                    }

                    if (projectId > 0)
                    {
                        var project = await _uow.ProjectRepository.GetSingleAsync(a => a.Id == projectId);
                        dto.ProjectUid = project.Uid;
                    }

                    uploadAdministrativeUnitDtos.Add(dto);

                    var tempResults = await _validator.ValidateCreate(dto);

                    if (tempResults.Count > 0)
                    {
                        tempResults = tempResults.Select(a => a += $" [row number {rowNum}]").ToList();
                        validationResults.AddRange(tempResults);
                    }
                    else
                    {
                        uploadAdministrativeUnits.Add(administrativeUnit);
                    }
                }
                else
                {
                    validationResults.Add(GetText("AdministrativeUnit_popup_txt_already_exists") +
                                          $" [row number {rowNum}]");
                }

                rowNum++;
            }

            validationResults = validationResults.Distinct().ToList();

            if (validationResults.Count > 0)
            {
                var errorJson =
                    JsonConvert.SerializeObject(new {message = string.Join("\n", validationResults)});
                return Content(errorJson, "application/json");
            }

            foreach (var val in uploadAdministrativeUnitDtos)
            {
                await _workflow.Create(UserId, val);
            }

            // await _uow.AdministrativeUnitRepository.AddRangeAsync(uploadAdministrativeUnits);

            var json = JsonConvert.SerializeObject(new {message = "success", status = 0});
            return Content(json, "application/json");
        }

        private async Task<List<Project>> GetProjectList()
        {
            var user = await _uow.AccountRepository.GetSingleAsync(u => u.Id == UserId);

            //assume one user can only administer one project.
            //super admin can select from multiple projects.
            return await _uow.ProjectRepository.GetAsync(
                predicate: p => (user.ProjectId == 0 || p.Id == user.ProjectId) &&
                                p.DeleteDate == null);
        }
    }
}