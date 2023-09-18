using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Bams.Workflows;
using Bams.Workflows.Enums;
using Bams.Workflows.InputValidators;
using Bams.Workflows.Models;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Helpers;
using BAMS.InputValidators;
using BAMS.Models;
using EightElements.Services;
using EightElements.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BAMS.Controllers
{
    public class UserAccountController : BaseController
    {
        private IStudentWorkflow _workflow;
        private IStudentValidator _validator;
        private IUnitOfWork _uow;
        private IMapper _mapper { get; set; }
        private readonly IConfiguration _config;
        private IChangelogService _changelog;

        private ITextService _textService;

        public UserAccountController(
            IStudentWorkflow workflow,
            IStudentValidator validator,
            IUnitOfWork unitOfWork,
            IConfiguration config,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            ITextService textService,
            IChangelogService changelogService
        ) : base(unitOfWork, textService)
        {
            _workflow = workflow;
            _validator = validator;
            _uow = unitOfWork;
            _config = config;
            _changelog = changelogService;
            _textService = textService;

            if (_mapper == null)
            {
                var configMapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserAccountDTO, UserAccount>();
                    cfg.CreateMap<UserAccount, UserAccountDTO>()
                        .ForMember(dest => dest.ActivationStatus, act => act.MapFrom(src => src.ActivationStatus == 1));
                    cfg.CreateMap<UserAccount, UserAccount>();
                    cfg.CreateMap<UserAccount, StudentDto>();
                });
                _mapper = configMapper.CreateMapper();
            }
        }

        // GET
        // [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
        //     Group = AccessGroups.UserAccount, Permission = Permissions.Read)]
        public async Task<IActionResult> Index(long uid = 0)
        {
            var student = await _uow.UserAccountRepository.GetByUidAsync(uid);
            ViewData["ProjectList"] = await GetProjectList();
            ViewData["DistrictList"] = await GetDistrictList();
            ViewData["AdmUnitList"] = await GetAdmUnitList();
            ViewData["SchoolList"] = await GetSchoolList();
            ViewData["Student"] = student;
            ViewData["DistrictId"] = DistrictId;
            ViewData["AdmUnitId"] = AdministrativeUnitId;
            ViewData["ProjectId"] = ProjectId;
            ViewData["RoleId"] = RoleId;
            ViewData["SchoolId"] = SchoolId;
            return View();
        }

        //API

        [HttpPost]
        public async Task<IActionResult> GetListUserAccount(DtParameters dtParameters)
        {
            try
            {
                var searchValue = dtParameters.Search.Value;
                int pageSize = dtParameters.Length;
                int skip = dtParameters.Start;
                int recordsFiltered = 0;
                int recordsTotal = 0;

                var pred = PredicateBuilder.True<UserAccount>();
                if (SchoolId > 0)
                {
                    pred = pred.And(a => a.SchoolId == SchoolId);
                }

                foreach (var col in dtParameters.Columns)
                {
                    if (!string.IsNullOrEmpty(col.Search.Value))
                    {
                        switch (col.Name)
                        {
                            case "Username":
                                pred = pred.And(a => a.UserName.Contains(col.Search.Value));
                                break;
                            case "Class":
                                pred = pred.And(a => a.Class.Contains(col.Search.Value));
                                break;
                            case "Name":
                                pred = pred.And(a => a.Name.Contains(col.Search.Value));
                                break;
                            case "PhoneNumber":
                                pred = pred.And(a => a.PhoneNumber.Contains(col.Search.Value));
                                break;
                            case "Email":
                                pred = pred.And(a => a.Email.Contains(col.Search.Value));
                                break;
                            case "ActivationStatus":
                                pred = pred.And(a => a.ActivationStatus.ToString().Contains(col.Search.Value));
                                break;
                            case "ProjectId":
                                pred = pred.And(a => a.ProjectId.ToString().Contains(col.Search.Value));
                                break;
                            case "SchoolId":
                                pred = pred.And(a => a.SchoolId.ToString().Contains(col.Search.Value));
                                break;
                        }
                    }
                }

                if (dtParameters.Columns.Length > 0)
                {
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        pred = pred.And(a => a.UserName.Contains(searchValue) || a.Name.Contains(searchValue) ||
                                             a.ProjectId.ToString().Contains(searchValue)
                                             || a.SchoolId.ToString()
                                                 .Contains(searchValue));
                    }
                }

                var colSort = dtParameters.Columns[dtParameters.Order[0].Column];
                var dir = dtParameters.Order[0].Dir;
                Expression<Func<UserAccount, dynamic>> sortFunc = a => a.Id;

                switch (colSort.Name)
                {
                    case "Name":
                        sortFunc = a => a.Name;
                        break;
                    case "Username":
                        sortFunc = a => a.UserName;
                        break;
                    case "Class":
                        sortFunc = a => a.Class;
                        break;
                    case "PhoneNumber":
                        sortFunc = a => a.PhoneNumber;
                        break;
                    case "Email":
                        sortFunc = a => a.Email;
                        break;
                    case "ActivationStatus":
                        sortFunc = a => a.ActivationStatus;
                        break;
                }

                Func<IQueryable<UserAccount>, IOrderedQueryable<UserAccount>> orderBy = o => o.OrderBy(sortFunc);
                if (dir == DtOrderDir.Desc)
                {
                    orderBy = o => o.OrderByDescending(sortFunc);
                }

                var data = await _uow.UserAccountRepository.GetAsync(orderBy, pred, true, pageSize, skip);
                recordsFiltered = await _uow.UserAccountRepository.CountAsync(pred);
                recordsTotal = await _uow.UserAccountRepository.CountAsync(a => true);

                var result = new DtResult<UserAccountDTO>()
                {
                    Data = _mapper.Map<List<UserAccountDTO>>(data),
                    Draw = dtParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal
                };

                var json = JsonConvert.SerializeObject(result);

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }


        [Route("get-student/{uid}")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.UserAccount,
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetStudent(long uid)
        {
            var student = await _uow.UserAccountRepository.GetByUidAsync(uid);
            if (student == null)
            {
                return Failed(GetText("Student_popup_txt_data_not_found"));
            }

            var dto = new
            {
                uid = student.Uid,
                schoolUid = await _uow.UserAccountRepository.GetUidById(student.SchoolId),
                className = student.Class,
                name = student.Name,
                username = student.UserName,
                phoneNumber = student.PhoneNumber,
                email = student.Email
            };

            return Success("student", JObject.FromObject(dto));
        }


        [Route("new-student")]
        [Route("student/{uid}")]
        [Route("student/detail")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.UserAccount,
            Permission = Permissions.Read)]
        public async Task<IActionResult> Detail(long uid = 0)
        {
            if (uid > 0)
            {
                try
                {
                    var student = await _uow.UserAccountRepository.GetByUidAsync(uid);

                    bool hasAccess = await HasAccess(student.SchoolId);
                    if (!hasAccess)
                    {
                        //todo: record to security log
                        return new BadRequestObjectResult(new
                            {message = GetText("Student_popup_txt_access_violation")});
                    }

                    ViewData["Student"] = student;
                }
                catch (Exception e)
                {
                    return new BadRequestObjectResult(new {message = e.Message});
                }
            }

            return View("Detail");
        }


        [Route("student/create")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.UserAccount,
            Permission = Permissions.Create)]
        public async Task<IActionResult> Create([FromBody] StudentDto dto)
        {
            var validationResults = await _validator.ValidateCreate(UserId, dto);
            if (validationResults.Count > 0)
            {
                string message = string.Join("<br/>", validationResults);
                return Failed(message);
            }

            var result = await _workflow.CreateStudent(UserId, dto);
            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                default:
                    return Failed(GetText("Student_popup_txt_unknow_error"));
            }

            return Content("ok");
        }


        [Route("student/update")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.UserAccount,
            Permission = Permissions.Update)]
        public async Task<IActionResult> Update([FromBody] StudentDto dto)
        {
            var validationResults = await _validator.ValidateUpdate(UserId, dto);
            if (validationResults.Count > 0)
            {
                string message = string.Join("<br/>", validationResults);
                return Failed(message);
            }

            var result = await _workflow.UpdateStudent(UserId, dto);
            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:
                    return Failed(GetText("Student_popup_txt_data_not_found"));
                default:
                    return Error(GetText("Student_popup_txt_unknow_error"));
            }
        }


        private async Task<bool> HasAccess(int schoolId)
        {
            var admin = await _uow.AccountRepository.GetByIdAsync(UserId);

            if (admin.SchoolId == schoolId)
            {
                return true;
            }

            return false;
        }


        [Route("delete-student/{uid}")]
        [Route("student/delete/{uid}")]
        [HttpGet]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.UserAccount,
            Permission = Permissions.Delete)]
        public async Task<IActionResult> Delete(long uid)
        {
            var result = await _workflow.DeleteStudent(UserId, uid);
            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:
                    return Failed(GetText("Student_popup_txt_data_not_found"));
                case WorkflowResult.AccessViolation:
                    return Failed(GetText("Student_popup_dont_have_permission_to_delete"));
                case WorkflowResult.ActionProhibited:
                    return Failed(GetText("Student_popup_cannot_deleted_still_active"));
                default:
                    return Error(GetText("Student_popup_txt_unknow_error"));
            }
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadStudents(string rawXls, int projectId = 0, int districtId = 0,
            int schoolId = 0, int admUnitId = 0)
        {
            if (projectId < 1) projectId = ProjectId;
            if (admUnitId < 1) admUnitId = AdministrativeUnitId;
            if (schoolId < 1) schoolId = SchoolId;

            var data = JsonConvert.DeserializeObject<List<UploadUserAccountDTO>>(rawXls);
            var duplicateData = data.GroupBy(a => a.Username).Where(a => a.Count() > 1).Select(a => a.Key).ToList();
            
            data = data.GroupBy(a => a.Username).Select(a => a.First()).ToList();
            var userNames = data.Select(a => a.Username).ToList();

            var validationResults = new List<string>();

            if (duplicateData.Count > 0)
            {
                var dupUserNames = string.Join(", ", duplicateData);
                validationResults.Add($"Duplicated username found [{dupUserNames}]");
            }

            if (projectId < 1)
            {
                validationResults.Add(GetText("Student_popup_txt_project_id_is_null"));
            }

            if (admUnitId < 1)
            {
                validationResults.Add(GetText("Student_popup_txt_district_id_is_null"));
            }

            if (schoolId < 1)
            {
                validationResults.Add(GetText("Student_popup_txt_school_id_is_null"));
            }

            var uploadedUserAccounts = new List<UserAccount>();
            var existedUsername = await _uow.UserAccountRepository.GetAsync(predicate: val =>
                val.ProjectId == ProjectId && val.AdministrativeUnitId == admUnitId && val.SchoolId == SchoolId &&
                userNames.Contains(val.UserName));

            var rowNum = 1;
            foreach (var upload in data)
            {
                if (string.IsNullOrEmpty(upload.StudentClass) || string.IsNullOrEmpty(upload.StudentFullName) ||
                    string.IsNullOrEmpty(upload.Username) || string.IsNullOrEmpty(upload.PhoneNumber))
                {
                    validationResults.Add(GetText("Student_popup_txt_please_use_correct_format") + $" [row number {rowNum}]");
                }

                try
                {
                    var m = new MailAddress(upload.Email);
                }
                catch (FormatException)
                {
                    validationResults.Add(GetText("Student_popup_txt_invalid_email") + $" [row number {rowNum}]");
                }

                var verifyPhoneNumber = PhoneNumberUtils.Verify(upload.PhoneNumber, "ID");
                if (verifyPhoneNumber.IsError)
                {
                    validationResults.Add(GetText("Student_popup_txt_invalid_phone_number") + $" [row number {rowNum}]");
                }

                var userAccount = existedUsername.Where(a => a.UserName.ToLower() == upload.Username.ToLower()).FirstOrDefault();

                if (userAccount == null)
                {
                    userAccount = new UserAccount();
                    userAccount.Class = upload.StudentClass;
                    userAccount.Name = upload.StudentFullName;
                    userAccount.Uid = await _uow.UserAccountRepository.GenerateUid();
                    userAccount.UserName = upload.Username;
                    userAccount.PhoneNumber = upload.PhoneNumber;
                    userAccount.Email = upload.Email;
                    userAccount.ProjectId = ProjectId;
                    userAccount.AdministrativeUnitId = AdministrativeUnitId;
                    userAccount.SchoolId = SchoolId;
                    userAccount.CreateDate = DateTime.Now;
                    userAccount.CreatedBy = UserId;

                    var dto = _mapper.Map<StudentDto>(userAccount);

                    var tempResults = await _validator.ValidateCreate(UserId, dto);

                    if(tempResults.Count > 0)
                    {
                        tempResults = tempResults.Select(a => a += $" [row number {rowNum}]").ToList();
                        validationResults.AddRange(tempResults);
                    }
                    else
                    {
                        uploadedUserAccounts.Add(userAccount);
                    }
                }
                else
                {
                    validationResults.Add(GetText("Student_popup_txt_student_is_already_exist") + $" [row number {rowNum}]");
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

            await _uow.UserAccountRepository.AddRangeAsync(uploadedUserAccounts);

            var json = JsonConvert.SerializeObject(new {message = GetText("success"), status = 0});
            return Content(json, "application/json");
        }


        [Route("activate-account/{uid}")]
        [HttpGet]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.UserAccount,
            Permission = Permissions.Update)]
        public async Task<IActionResult> ActivateAccount(long uid)
        {
            var result = await _workflow.ActivateAccount(UserId, uid);
            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:
                    return Failed(GetText("Student_popup_txt_data_not_found"));
                case WorkflowResult.AccessViolation:
                    return Failed(GetText("Student_popup_txt_access_violation"));
                case WorkflowResult.ActivationCodeNotFound:
                    return Failed("Student_popup_activation_code_is_not_available");
                default:
                    return Error(GetText("Student_popup_txt_unknow_error"));
            }
        }


        [Route("dismiss-class/{className}")]
        [HttpGet]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.UserAccount,
            Permission = Permissions.Delete)]
        public async Task<IActionResult> DismissClass(string className)
        {
            var result = await _workflow.DismissClass(UserId, className);
            switch (result)
            {
                case WorkflowResult.Success:
                    return Success();
                default:
                    return Error(GetText("Student_popup_txt_unknow_error"));
            }
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

        private async Task<List<AdministrativeUnit>> GetAdmUnitList(int projectId = 0, int admUnit = 0)
        {
            var pred = PredicateBuilder.True<AdministrativeUnit>();
            if (projectId > 0)
            {
                pred = pred.And(a => a.ProjectId == projectId);
            }

            if (admUnit > 0)
            {
                pred = pred.And(a => a.Id == admUnit);
            }

            var data = await _uow.AdministrativeUnitRepository.GetAsync(predicate: pred,
                orderBy: o => o.OrderBy(a => a.Name));

            if (projectId == 0 && admUnit == 0)
            {
                data = data.Prepend(new AdministrativeUnit { Id = 0, Name = "ALL Districts" }).ToList();
            }

            return data;
        }

        private async Task<List<School>> GetSchoolList()
        {
            var user = await _uow.AccountRepository.GetSingleAsync(u => u.Id == UserId);

            if (user.ProjectId == 0)
            {
                //super user -> show all district
                return await _uow.SchoolRepository.GetAsync(
                    predicate: d => d.DeleteDate == null);
            }

            if (user.ProjectId != null && user.DistrictId == null)
            {
                //user is project admin -> show all districts in the project
                return await _uow.SchoolRepository.GetAsync(
                    predicate: d =>
                        d.ProjectId == user.ProjectId && d.DeleteDate == null);
            }

            if (user.DistrictId != null)
            {
                //user is district admin -> only show single district
                return await _uow.SchoolRepository.GetAsync(
                    predicate: d =>
                        d.DistrictId == user.DistrictId && d.DeleteDate == null);
            }

            if (user.SchoolId != null)
            {
                //user is district admin -> only show single district
                return await _uow.SchoolRepository.GetAsync(
                    predicate: d =>
                        d.Id == user.SchoolId && d.DeleteDate == null);
            }

            return null; //access violation
        }
    }
}