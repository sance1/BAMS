using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Helpers;
using BAMS.InputValidators;
using BAMS.Models;
using EightElements.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Bams.Workflows;
using Bams.Workflows.InputValidators;
using Bams.Workflows.Models;
using Bams.Workflows.Enums;

namespace BAMS.Controllers
{
    public class SchoolController : BaseController
    {
        private ISchoolWorkflow _workflow;
        private ISchoolValidator _validator;
        private IUnitOfWork _uow;
        private IMapper _mapper { get; set; }
        private readonly ILogger<SchoolController> _logger;
        

        private ITextService _textService;

        public SchoolController(
            ISchoolWorkflow workflow,
            ISchoolValidator validator,
            IUnitOfWork unitOfWork,
            ILogger<SchoolController> logger,
            ITextService textService
        ) : base(unitOfWork, textService)
        {
            _workflow = workflow;
            _validator = validator;
            _uow = unitOfWork;
            _logger = logger;        
            _textService = textService;

            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ReadSchoolDto, School>();
                    cfg.CreateMap<School, ReadSchoolDto>();
                    cfg.CreateMap<School, School>();
                    cfg.CreateMap<School, SchoolDto>();
                });

                _mapper = config.CreateMapper();
            }
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.School, Permission = Permissions.Read)]
        public async Task<IActionResult> Index(long uid = 0, long districtUid = 0, long admUnitUid = 0)
        {
            //if (DistrictId > 0)
            //{
            //    var district = await _uow.DistrictRepository.GetSingleAsync(a => a.Id == DistrictId);
            //    ViewData["district"] = district;
            //}
            if (AdministrativeUnitId > 0)
            {
                var admUnit = await _uow.AdministrativeUnitRepository.GetSingleAsync(a => a.Id == AdministrativeUnitId);
                ViewData["admUnit"] = admUnit;
            }

            ViewData["ProjectList"] = await GetProjectList();
            //ViewData["DistrictList"] = await GetDistrictList();
            ViewData["AdmUnitList"] = await GetAdmUnitList();
            ViewData["SchoolList"] = await GetSchoolList();

            //ViewData["DistrictUid"] = districtUid;
            //ViewData["DistrictId"] = DistrictId;

            ViewData["AdmUnitUid"] = admUnitUid;
            ViewData["AdmUnitId"] = AdministrativeUnitId;

            ViewData["ProjectId"] = ProjectId;
            ViewData["RoleId"] = RoleId;

            if (uid > 0)
            {
                try
                {
                    var school = await _uow.SchoolRepository.GetByUidAsync(uid);
                    bool hasAccess = await HasAccess(school.DistrictId);
                    if (!hasAccess)
                    {
                        //todo: record to security log
                        return new BadRequestObjectResult(new
                            {message = GetText("School_popup_txt_access_violation")});
                    }

                    ViewData["school"] = school;
                    //ViewData["DistrictUid"] = await _uow.DistrictRepository.GetUidById(school.DistrictId);
                    ViewData["AdmUnitUid"] = await _uow.AdministrativeUnitRepository.GetUidById(school.AdministrativeUnitId);
                }
                catch (Exception e)
                {
                    _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                    return new BadRequestObjectResult(new {message = e.Message});
                }
            }

            return View();
        }

        [HttpPost]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.School, Permission = Permissions.Read)]
        public async Task<IActionResult> GetListSchool(DtParameters dtParameters)
        {
            try
            {
                var searchValue = dtParameters.Search.Value;
                int pageSize = dtParameters.Length;
                int skip = dtParameters.Start;
                int recordsTotal = 0;

                var pred = PredicateBuilder.True<School>();
                if (ProjectId > 0)
                {
                    pred = pred.And(a => a.ProjectId == ProjectId);
                }

                if (DistrictId > 0)
                {
                    pred = pred.And(a => a.DistrictId == DistrictId);
                }

                if (AdministrativeUnitId > 0)
                {
                    pred = pred.And(a => a.AdministrativeUnitId == AdministrativeUnitId);
                }

                foreach (var col in dtParameters.Columns)
                {
                    if (!string.IsNullOrEmpty(col.Search.Value))
                    {
                        switch (col.Name)
                        {
                            case "Name":
                                pred = pred.And(a => a.Name.Contains(col.Search.Value));
                                break;
                            case "DistrictName":
                                pred = pred.And(a => a.District.Name.Contains(col.Search.Value));
                                break;
                            case "Address":
                                pred = pred.And(a => a.Address.Contains(col.Search.Value));
                                break;
                            case "PIC":
                                pred = pred.And(a => a.PIC.Contains(col.Search.Value));
                                break;
                            case "Students":
                                pred = pred.And(a => a.Students.ToString().Contains(col.Search.Value));
                                break;
                            case "Remarks":
                                pred = pred.And(a => a.Remarks.Contains(col.Search.Value));
                                break;
                        }
                    }
                }

                var colSort = dtParameters.Columns[dtParameters.Order[0].Column];
                var dir = dtParameters.Order[0].Dir;
                Expression<Func<School, dynamic>> sortFunc = a => a.Id;

                switch (colSort.Name)
                {
                    case "Name":
                        sortFunc = a => a.Name;
                        break;
                    case "DistrictName":
                        sortFunc = a => a.AdministrativeUnit.Name;
                        break;
                    case "Address":
                        sortFunc = a => a.Address;
                        break;
                    case "PIC":
                        sortFunc = a => a.PIC;
                        break;
                    case "Students":
                        sortFunc = a => a.Students;
                        break;
                    case "Remarks":
                        sortFunc = a => a.Remarks;
                        break;
                }

                Func<IQueryable<School>, IOrderedQueryable<School>> orderBy = o => o.OrderBy(sortFunc);
                if (dir == DtOrderDir.Desc)
                {
                    orderBy = o => o.OrderByDescending(sortFunc);
                }

                var data = await _uow.SchoolRepository
                    .GetAsync(selector: a => new ReadSchoolDto()
                        {
                            Uid = a.Uid.ToString(),
                            Id = a.Id,
                            Address = a.Address,
                            ProjectId = a.ProjectId,
                            DistrictId = a.DistrictId,
                            Students = a.Students,
                            Name = a.Name,
                            PIC = a.PIC,
                            Remarks = a.Remarks,
                            ActivationCodes = a.ActivationCodes.Count,
                            DistrictName = a.AdministrativeUnit.Name,
                            AdminstrativeUnitId = a.AdministrativeUnit.Id
                    },
                        orderBy, pred, true, pageSize, skip, includeProperties: "ActivationCodes,AdministrativeUnit");
                var recordsFiltered = await _uow.SchoolRepository.CountAsync(pred);
                recordsTotal = await _uow.SchoolRepository.CountAsync(pred);

                var result = new DtResult<ReadSchoolDto>()
                {
                    Data = data,
                    Draw = dtParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal
                };

                var json = JsonConvert.SerializeObject(result);

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error while get data | {e.Message}", e);
                return new BadRequestObjectResult(e.Message);
            }
        }


        [Route("get-school/{uid}")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.School,
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetSchool(long uid)
        {
            try
            {
                var school = await _uow.SchoolRepository.GetByUidAsync(uid);
                if (school == null)
                {
                    return Failed(GetText("School_popup_txt_school_not_found"));
                }

                var dto = new
                {
                    uid = school.Uid,
                    districtUid = await _uow.DistrictRepository.GetUidById(school.DistrictId),
                    admUnitUid = await _uow.AdministrativeUnitRepository.GetUidById(school.AdministrativeUnitId),
                    name = school.Name,
                    address = school.Address,
                    students = school.Students,
                    picName = school.PIC,
                    remarks = school.Remarks
                };
                return Success("school", JObject.FromObject(dto));
            }

            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(new {message = e.Message});
            }
        }


        [Route("new-school")]
        [Route("school/{uid}")]
        [Route("school/detail")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.School,
            Permission = Permissions.Create)]
        public async Task<IActionResult> Detail(long uid = 0, long districtUid = 0)
        {
            ViewData["DistrictList"] = await GetDistrictList();
            ViewData["AdmUnitList"] = await GetAdmUnitList();
            ViewData["DistrictUid"] = districtUid;

            if (uid > 0)
            {
                try
                {
                    var school = await _uow.SchoolRepository.GetByUidAsync(uid);
                    bool hasAccess = await HasAccess(school.DistrictId);
                    if (!hasAccess)
                    {
                        //todo: record to security log
                        return new BadRequestObjectResult(new
                            {message = GetText("School_popup_txt_access_violation")});
                    }

                    ViewData["school"] = school;
                    ViewData["DistrictUid"] = await _uow.DistrictRepository.GetUidById(school.DistrictId);
                }
                catch (Exception e)
                {
                    _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                    return new BadRequestObjectResult(new {message = e.Message});
                }
            }

            return View("CreateUpdate");
        }


        [Route("school/create")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.School,
            Permission = Permissions.Create)]
        public async Task<IActionResult> Create([FromBody] SchoolDto dto)
        {
            var validationResults = await _validator.ValidateCreate(UserId, dto,GetClientLanguage());
            if (validationResults.Count > 0)
            {
                string message = string.Join("<br/>", validationResults);
                return Failed(message);
            }

            var result = await _workflow.CreateSchool(UserId, dto);
            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                default:
                    return Failed(GetText("School_popup_txt_unknow_error"));
            }
        }


        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadSchools(string rawXls, int projectId = 0, int districtId = 0, int admUnitId = 0)
        {
            if (projectId < 1) projectId = ProjectId;
            if(admUnitId < 1) admUnitId = AdministrativeUnitId;

            var data = JsonConvert.DeserializeObject<List<UploadSchoolDto>>(rawXls);
            var duplicateData = data.GroupBy(a => a.SchoolName).Where(a => a.Count() > 1).Select(a => a.Key).ToList();

            data = data.GroupBy(a => a.SchoolName).Select(a => a.First()).ToList();
            var schoolNames = data.Select(a => a.SchoolName).ToList();

            var validationResults = new List<string>();

            if (duplicateData.Count > 0)
            {
                var dupSchool = string.Join(", ", duplicateData);
                validationResults.Add($"Duplicated School Name found [{dupSchool}]");
            }

            if (projectId < 1)
            {
                validationResults.Add(_textService.GetString("School_popup_txt_project_id_null", "en"));
            }

            if (admUnitId < 1)
            {
                validationResults.Add(_textService.GetString("School_popup_txt_district_id_null", "en"));
            }

            var uploadedSchool = new List<School>();
            var existedSchool = await _uow.SchoolRepository
                .GetAsync(predicate: a => a.ProjectId == projectId && a.AdministrativeUnitId == admUnitId && schoolNames.Contains(a.Name));

            var adminUnitUid = await _uow.AdministrativeUnitRepository.GetUidById(admUnitId); 

            var rowNum = 1;
            foreach (var upload in data)
            {
                if (string.IsNullOrEmpty(upload.Address) || string.IsNullOrEmpty(upload.SchoolName) ||
                    string.IsNullOrEmpty(upload.PIC))
                {
                    validationResults.Add(GetText("School_popup_txt_incorect_format_upload") + $" [row number {rowNum}]");
                }

                if (upload.SchoolName.Length < 6)
                {
                    validationResults.Add(GetText("School_popup_txt_invalid_name") + $" [row number {rowNum}]");
                }

                var school = existedSchool.Where(a => a.Name.ToLower() == upload.SchoolName.ToLower()).FirstOrDefault();

                if (school == null)
                {
                    school = new School();
                    school.Name = upload.SchoolName;
                    school.PIC = upload.PIC;
                    school.Address = upload.Address;
                    school.Uid = await _uow.SchoolRepository.GenerateUid();
                    school.ProjectId = projectId;
                    school.AdministrativeUnitId = admUnitId;
                    school.Students = upload.TotalStudent;
                    school.CreateDate = DateTime.Now;
                    school.CreatedBy = UserId;

                    var dto = _mapper.Map<SchoolDto>(school);
                    dto.AdmUnitUid = adminUnitUid;

                    var tempResults = await _validator.ValidateCreate(UserId, dto, GetClientLanguage());
                    
                    if (tempResults.Count > 0)
                    {
                        tempResults = tempResults.Select(a => a += $" [row number {rowNum}]").ToList();
                        validationResults.AddRange(tempResults);
                    }else
                    {
                        uploadedSchool.Add(school);
                    }
                }
                else
                {
                    validationResults.Add(GetText("School_popup_txt_already_exists") + $" [row number {rowNum}]");
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

            await _uow.SchoolRepository.AddRangeAsync(uploadedSchool);

            var json = JsonConvert.SerializeObject(new {message = "success", status = 0});
            return Content(json, "application/json");
        }


        private async Task<bool> HasAccess(int districtId)
        {
            var user = await _uow.AccountRepository.GetByIdAsync(UserId);

            if (user.ProjectId == 0)
            {
                return true; // super-user
            }

            var district = await _uow.DistrictRepository.GetByIdAsync(districtId);

            if (user.ProjectId == district.ProjectId)
            {
                //user is project admin -> allow access
                return true;
            }

            if (user.DistrictId == districtId)
            {
                //user is district admin -> allow access
                return true;
            }

            return false;
        }


        [Route("school/update")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.School,
            Permission = Permissions.Update)]
        public async Task<IActionResult> Update([FromBody] SchoolDto dto)
        {
            var validationResults = await _validator.ValidateUpdate(UserId, dto,GetClientLanguage());
            if (validationResults.Count > 0)
            {
                return Failed(string.Join("<br/>", validationResults));
            }

            var result = await _workflow.UpdateSchool(UserId, dto);
            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:
                    return Failed(GetText("School_popup_txt_data_not_found"));
                case WorkflowResult.ActionProhibited:
                    return Failed(GetText("School_popup_txt_there_is_still_student_accounts"));
                default:
                    return Error(GetText("School_popup_txt_unknown_error"));
            }
        }


        [Route("delete-school/{uid}")]
        [Route("school/delete/{uid}")]
        [HttpGet]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.School,
            Permission = Permissions.Delete)]
        public async Task<IActionResult> Delete(long uid)
        {
            var result = await _workflow.DeleteSchool(UserId, uid);

            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:
                    return Failed(GetText("School_popup_txt_data_not_found"));
                case WorkflowResult.AccessViolation:
                    return Failed(GetText("School_popup_txt_dont_have_permission_to_delete"));
                case WorkflowResult.ActionProhibited:
                    return Failed(GetText("School_popup_txt_cant_delete_thereisstudent"));
                default:
                    return Error(GetText("School_popup_txt_unknown_error"));
            }
        }


        private async Task<List<District>> GetDistrictList()
        {
            var user = await _uow.AccountRepository.GetSingleAsync(u => u.Id == UserId);

            if (user.ProjectId == 0)
            {
                //super user -> show all district
                return await _uow.DistrictRepository.GetAsync(
                    predicate: d => d.DeleteDate == null);
            }

            if (user.ProjectId != null && user.DistrictId == null)
            {
                //user is project admin -> show all districts in the project
                return await _uow.DistrictRepository.GetAsync(
                    predicate: d =>
                        d.ProjectId == user.ProjectId && d.DeleteDate == null);
            }

            if (user.DistrictId != null)
            {
                //user is district admin -> only show single district
                return await _uow.DistrictRepository.GetAsync(
                    predicate: d =>
                        d.Id == user.DistrictId && d.DeleteDate == null);
            }

            return null; //access violation
        }

        private async Task<List<AdministrativeUnit>> GetAdmUnitList()
        {
            var user = await _uow.AccountRepository.GetSingleAsync(u => u.Id == UserId);

            if (user.ProjectId == 0)
            {
                //super user -> show all 
                return await _uow.AdministrativeUnitRepository.GetAsync(
                    predicate: d => d.DeleteDate == null);
            }

            if (user.ProjectId != null && user.AdministrativeUnitId == null)
            {
                //user is project admin -> show all in the project
                return await _uow.AdministrativeUnitRepository.GetAsync(
                    predicate: d =>
                        d.ProjectId == user.ProjectId && d.DeleteDate == null);
            }

            if (user.AdministrativeUnitId != null)
            {
                //user is district admin -> only show single 
                return await _uow.AdministrativeUnitRepository.GetAsync(
                    predicate: d =>
                        d.Id == user.AdministrativeUnitId && d.DeleteDate == null);
            }

            return null; //access violation
        }

        private async Task<List<ReadSchoolDto>> GetSchoolList()
        {
            var user = await _uow.AccountRepository.GetSingleAsync(u => u.Id == UserId);

            if (user.ProjectId == 0)
            {
                //super user -> show all school
                return await _uow.SchoolRepository
                    .GetAsync(selector: a => new ReadSchoolDto()
                        {
                            Uid = a.Uid.ToString(),
                            Name = a.Name,
                            ActivationCodes = a.ActivationCodes.Count()
                        },
                        predicate: d => d.DeleteDate == null, includeProperties: "ActivationCodes");
            }

            if (user.ProjectId != null && user.DistrictId == null)
            {
                //user is project admin -> show all school in the project
                return await _uow.SchoolRepository
                    .GetAsync(selector: a => new ReadSchoolDto()
                    {
                        Uid = a.Uid.ToString(),
                        Name = a.Name,
                        ActivationCodes = a.ActivationCodes.Count()
                    },
                        predicate: d => d.ProjectId == user.ProjectId && d.DeleteDate == null);
            }

            if (user.AdministrativeUnitId != null)
            {
                //user is district admin -> only show school on that district
                return await _uow.SchoolRepository
                    .GetAsync(selector: a => new ReadSchoolDto()
                     {
                         Uid = a.Uid.ToString(),
                         Name = a.Name,
                         ActivationCodes = a.ActivationCodes.Count()
                     },
                    predicate: d =>
                        d.ProjectId == user.ProjectId &&  d.AdministrativeUnitId == user.DistrictId && d.DeleteDate == null);
            }

            if (user.SchoolId != null)
            {
                //user is school admin -> only show single school
                return await _uow.SchoolRepository
                    .GetAsync(selector: a => new ReadSchoolDto()
                    {
                        Uid = a.Uid.ToString(),
                        Name = a.Name,
                        ActivationCodes = a.ActivationCodes.Count()
                    },
                    predicate: d =>
                        d.Id == user.SchoolId && d.DeleteDate == null);
            }

            return null; //access violation
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.School, Permission = Permissions.Update)]
        public async Task<IActionResult> AllocateCode(long schoolUid, int qty,
            [FromServices] IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                var school = await _uow.SchoolRepository.GetByUidAsync(schoolUid);
                if (school == null)
                {
                    return new NotFoundObjectResult(new {message = GetText("School_popup_txt_data_not_found")});
                }

                bool hasAccess = await HasAccess(school.DistrictId);
                if (!hasAccess)
                {
                    //todo: record to security log
                    return new BadRequestObjectResult(new {message = GetText("School_popup_txt_access_violation")});
                }

                var contract = (await _uow.ContractRepository
                        .GetAsync(predicate: a => a.ProjectId == school.ProjectId && a.Id == ContractId,
                            includeProperties: "ActivationCodeRequest"))
                    .SingleOrDefault();
                if (contract == null)
                {
                    return new NotFoundObjectResult(new
                        {message = GetText("School_popup_txt_dont_have_access")});
                }

                if (contract.ActivationCodeRequest == null)
                {
                    return new NotFoundObjectResult(new
                        {message = GetText("School_popup_txt_activation_not_available")});
                }

                if (contract.ActivationCodes != contract.ActivationCodeRequest.AmountCodes)
                {
                    return new NotFoundObjectResult(new
                        {message = GetText("School_popup_txt_activation_not_available")});
                }

                if (qty > contract.ActivationCodes)
                {
                    return new BadRequestObjectResult(new
                        {message = GetText("School_popup_txt_amount_not_valid")});
                }

                var schoolCodesQty = await _uow.activationCodeRepository
                    .CountAsync(a =>
                        a.ProjectId == contract.ProjectId && a.ContractId == contract.Id &&
                        a.AdministrativeUnitId == AdministrativeUnitId && a.SchoolId == school.Id);

                qty = qty - schoolCodesQty;

                if (qty <= 0)
                {
                    return new BadRequestObjectResult(new
                        {message = GetText("School_popup_txt_amount_not_valid")});
                }

                var codesQty = await _uow.activationCodeRepository
                    .CountAsync(a =>
                        a.ProjectId == contract.ProjectId && a.ContractId == contract.Id &&
                        a.DistrictId == DistrictId && a.SchoolId == 0 && a.UserAccountId == 0);
                if (codesQty < qty)
                {
                    return new NotFoundObjectResult(new {message = GetText("School_popup_txt_request_rejected")});
                }

                _ = Task.Run(async () =>
                {
                    try
                    {
                        var schoolId = school.Id;
                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                            var codes = await uow.activationCodeRepository
                                .GetAsync(predicate: a => a.ProjectId == contract.ProjectId &&
                                                          a.ContractId == contract.Id
                                                          && a.AdministrativeUnitId == AdministrativeUnitId && a.SchoolId == 0 &&
                                                          a.UserAccountId == 0,
                                    usePaging: true, pageSize: qty);
                            foreach (var code in codes)
                            {
                                code.SchoolId = schoolId;
                            }

                            await uow.SaveAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"error while allocate code school | {e.Message}");
                    }
                });

                return new OkObjectResult(new {status = 0, message = GetText("School_popup_txt_processing")});
            }
            catch (Exception e)
            {
                _logger.LogError($"error while allocate code district | {e.Message}");
                return new BadRequestObjectResult(e.Message);
            }
        }

        private async Task<List<Project>> GetProjectList()
        {
            var user = await _uow.AccountRepository.GetByIdAsync(UserId);

            //assume one user can only administer one project.
            //super admin can select from multiple projects.
            return await _uow.ProjectRepository.GetAsync(
                predicate: p => (user.ProjectId == 0 || p.Id == user.ProjectId) &&
                                p.DeleteDate == null);
        }
    }
}