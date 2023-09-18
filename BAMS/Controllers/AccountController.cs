using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Helpers;
using BAMS.Models;
using EightElements.Services;
using EightElements.Services.Models;
using EightElements.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BAMS.Controllers
{
    public class AccountController : BaseController
    {
        private IUnitOfWork unitOfWork;
        private IMapper _mapper { get; set; }
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;
        private readonly ITextService _textService;


        public AccountController(IUnitOfWork unitOfWork,
            IConfiguration config,
            IMailService mailService,
            ILogger<AccountController> logger,
            ITextService textService
        ) : base(unitOfWork, textService)
        {
            this.unitOfWork = unitOfWork;
            _config = config;
            _logger = logger;
            _textService = textService;

            if (_mapper == null)
            {
                var configMapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ReadAccountDTO, Account>();
                    cfg.CreateMap<Account, ReadAccountDTO>()
                        .ForMember(dest => dest.RoleName,
                            act => act.MapFrom(src => src.Role == null ? "" : src.Role.Name))
                        .ForMember(dest => dest.ProjectName,
                            act => act.MapFrom(src => src.Project == null ? "" : src.Project.Name));
                    ;
                    cfg.CreateMap<Account, Account>();
                    cfg.CreateMap<District, ReadDistrictDto>();
                    cfg.CreateMap<Contract, ReadContractDto>();
                    cfg.CreateMap<School, ReadSchoolDto>();
                    cfg.CreateMap<AdministrativeUnit, AdministrativeUnitModel>();
                    cfg.CreateMap<AdministrativeUnit, AdmUnitDTO>();
                });

                _mapper = configMapper.CreateMapper();
                _mailService = mailService;
            }
        }

        public async Task<IActionResult> GetMenuConfig(string menu = "")
        {
            var json = MenuConfiguration.GetMenu(_config, menu, RoleId);
            foreach (var headerTable in json)
            {
                headerTable.Name = GetText(headerTable.Name);
            }

            return Content(JsonConvert.SerializeObject(json), "application/json");
        }

        // GET

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Account, Permission = Permissions.Read)]
        public async Task<IActionResult> Index(int id = 0)
        {
            ViewBag.Id = id;

            ViewBag.Roles = await GetListRoles();

            var projects = await GetListProjects(ProjectId);
            ViewBag.Projects = projects;

            var contracts = await GetListContracts(ContractId);
            ViewBag.Contracts = contracts;

            var districts = await GetListDistrict(ProjectId, DistrictId);
            ViewBag.Districts = districts;

            var admUnits = await GetListAdmUnit(ProjectId, AdministrativeUnitId);
            ViewBag.AdmUnits = admUnits;

            var schools = await GetListSchool(ProjectId, AdministrativeUnitId, SchoolId);
            ViewBag.Schools = schools;

            if (id > 0)
            {
                ViewBag.Account = await unitOfWork.AccountRepository.GetSingleAsync(ar => ar.Id == id);
            }
            else
            {
                ViewBag.Account = new Account();
            }

            ViewData["UserRole"] = RoleId;
            ViewData["ContractId"] = ContractId;
            ViewData["ProjectId"] = ProjectId;

            return View();
        }

        [Route("get-account/{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            var account = await unitOfWork.AccountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return Failed(GetText("Account_popup_txt_account_not_found"));
            }

            var dto = new
            {
                id = account.Id,
                username = account.UserName,
                email = account.Email,
                project = account.ProjectId,
                contract = account.ContractId,
                district = account.DistrictId,
                admUnit = account.AdministrativeUnitId,
                school = account.SchoolId,
                role = account.RoleId,
                organization = account.Organization
            };
            ViewBag.Id = id;
            return Success("account", JObject.FromObject(dto));
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Account, Permission = Permissions.Create)]
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.Id = 0;
                ViewBag.Account = new Account();
                ViewBag.Roles = await GetListRoles();

                var projects = await GetListProjects(ProjectId);
                ViewBag.Projects = projects;

                var districts = await GetListDistrict(ProjectId, DistrictId);
                ViewBag.Districts = districts;

                return View("CreateOrUpdate");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                ViewBag.Message = e.Message;
                return View("Views/Error/Index.cshtml");
            }
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Account, Permission = Permissions.Update)]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                ViewBag.Id = id;
                ViewBag.Account = await unitOfWork.AccountRepository.GetSingleAsync(ar => ar.Id == id);
                ViewBag.Roles = await GetListRoles();

                var projects = await GetListProjects(ProjectId);
                ViewBag.Projects = projects;

                var districts = await GetListDistrict(ProjectId, DistrictId);
                ViewBag.Districts = districts;

                return View("CreateOrUpdate");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                ViewBag.Message = e.Message;
                return View("Views/Error/Index.cshtml");
            }
        }


        [Route("Login")]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("change-password/{token}")]
        public async Task<IActionResult> ChangePassword(string token)
        {
            try
            {
                var decode = RandomGenerator.Base64Decode(token);
                var split = decode.Split(".");
                var date = split[0];
                DateTime dt = DateTime.Parse(date);
                var now = DateTime.Now;
                if (now > dt)
                {
                    return Content(GetText("Change_password_popup_txt_expired"));
                }

                ViewBag.Uid = split[split.Length - 1];

                return View();
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                ViewBag.Message = e.Message;
                return View("Views/Error/Index.cshtml");
            }
        }

        private async Task<List<Role>> GetListRoles()
        {
            if (RoleId > 1)
            {
                var role = await unitOfWork.RoleRepository.GetSingleAsync(p => p.Id == RoleId);
                return await unitOfWork.RoleRepository.GetAsync(null, p => p.AccessLevel > role.AccessLevel);
            }

            return await unitOfWork.RoleRepository.GetAsync();
        }

        private async Task<List<Project>> GetListProjects(int projectId = 0)
        {
            var pred = PredicateBuilder.True<Project>();
            if (projectId > 0)
            {
                pred = pred.And(a => a.Id == projectId);
            }

            var data = await unitOfWork.ProjectRepository.GetAsync(predicate: pred,
                orderBy: o => o.OrderBy(a => a.Name));

            if (projectId == 0)
            {
                data = data.Prepend(new Project {Id = 0, Name = _textService.GetString("Account_form_dropdown_all_project", "en")}).ToList();
            }

            return data;
        }

        private async Task<List<Contract>> GetListContracts(int projectId = 0, int contractId = 0)
        {
            var pred = PredicateBuilder.True<Contract>();
            if (projectId > 0)
            {
                pred = pred.And(a => a.ProjectId == projectId);
            }

            if (contractId > 0)
            {
                pred = pred.And(a => a.Id == contractId);
            }

            var data = await unitOfWork.ContractRepository.GetAsync(predicate: pred,
                orderBy: o => o.OrderBy(a => a.Name));

            if (contractId == 0)
            {
                data = data.Prepend(new Contract {Id = 0, Name = _textService.GetString("Account_form_dropdown_all_contracts","en") }).ToList();
            }

            return data;
        }

        private async Task<List<District>> GetListDistrict(int projectId = 0, int districtId = 0)
        {
            var pred = PredicateBuilder.True<District>();
            if (projectId > 0)
            {
                pred = pred.And(a => a.ProjectId == projectId);
            }

            if (districtId > 0)
            {
                pred = pred.And(a => a.Id == districtId);
            }

            var data = await unitOfWork.DistrictRepository.GetAsync(predicate: pred,
                orderBy: o => o.OrderBy(a => a.Name));

            if (projectId == 0 && districtId == 0)
            {
                data = data.Prepend(new District {Id = 0, Name = _textService.GetString("Account_form_dropdown_all_districts","en") }).ToList();
            }

            return data;
        }

        private async Task<List<AdministrativeUnit>> GetListAdmUnit(int projectId = 0, int admUnit = 0)
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

            var data = await unitOfWork.AdministrativeUnitRepository.GetAsync(predicate: pred,
                orderBy: o => o.OrderBy(a => a.Name));

            if (projectId == 0 && admUnit == 0)
            {
                data = data.Prepend(new AdministrativeUnit { Id = 0, Name = "ALL Districts" }).ToList();
            }

            return data;
        }

        private async Task<List<School>> GetListSchool(int projectId = 0, int admUnitId = 0, int schoolId = 0)
        {
            var pred = PredicateBuilder.True<School>();
            if (projectId > 0)
            {
                pred = pred.And(a => a.ProjectId == projectId);
            }

            if (admUnitId > 0)
            {
                pred = pred.And(a => a.AdministrativeUnitId == admUnitId);
            }

            if (schoolId > 0)
            {
                pred = pred.And(a => a.Id == schoolId);
            }

            var data = await unitOfWork.SchoolRepository.GetAsync(predicate: pred,
                orderBy: o => o.OrderBy(a => a.Name));

            if (projectId == 0 && admUnitId == 0)
            {
                data = data.Prepend(new School {Id = 0, Name = _textService.GetString("Account_form_dropdown_empty","en") }).ToList();
            }

            return data;
        }

        [HttpGet]
        public async Task<IActionResult> GetDistrict(int projectId)
        {
            try
            {
                var data = await GetListDistrict(projectId);

                var result = _mapper.Map<List<ReadDistrictDto>>(data);

                var json = JsonConvert.SerializeObject(result);

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAdmUnit(int projectId)
        {
            try
            {
                var data = await GetListAdmUnit(projectId);

                var result = _mapper.Map<List<AdmUnitDTO>>(data);

                var json = JsonConvert.SerializeObject(result);

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetContract(int projectId)
        {
            try
            {
                var data = await GetListContracts(projectId);

                var result = _mapper.Map<List<ReadContractDto>>(data);

                var json = JsonConvert.SerializeObject(result);

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSchool(int projectId, int districtId = 0)
        {
            try
            {
                var data = await GetListSchool(projectId, districtId);

                var result = _mapper.Map<List<ReadSchoolDto>>(data);

                var json = JsonConvert.SerializeObject(result);

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(e.Message);
            }
        }

        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var role = await unitOfWork.AccountRepository.GetSingleAsync(ac => ac.Id == id);
                role.DeleteDate = DateTime.Now;
                role.DeletedBy = 1;
                await unitOfWork.SaveAsync();
                return Content("ok");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string uid, string password)
        {
            try
            {
                long uidLong = long.Parse(uid);
                var account = await unitOfWork.AccountRepository.GetSingleAsync(a => a.Uid == uidLong);
                if (account != null)
                {
                    if (password.Length < 6 || password.Length > 12)
                    {
                        return Content(GetText("Change_password_popup_at_least_character"));
                    }

                    if (!password.All(IsLetterOrDigit))
                    {
                        return Content(GetText("Change_password_popup_at_least_character"));
                    }

                    var option = new HashingOptions();
                    PasswordHasher hasher = new PasswordHasher(new OptionsWrapper<HashingOptions>(option));
                    var newPassword = hasher.Hash(password);
                    account.Password = newPassword;
                    await unitOfWork.SaveAsync();
                }

                return Content("ok");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(e.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetListAccount(DtParameters dtParameters)
        {
            try
            {
                var searchValue = dtParameters.Search.Value;
                int pageSize = dtParameters.Length;
                int skip = dtParameters.Start;
                int recordsFiltered = 0;
                int recordsTotal = 0;

                var pred = PredicateBuilder.True<Account>();

                if (ProjectId > 0)
                {
                    pred = pred.And(a => a.ProjectId == ProjectId);
                }

                if (DistrictId > 0)
                {
                    pred = pred.And(a => a.DistrictId == DistrictId);
                }

                if (UserId > 0)
                {
                    pred = pred.And(a => a.Id != UserId);
                }

                var accesLevel = await unitOfWork.RoleRepository.GetByIdAsync(RoleId);
                if (accesLevel != null)
                {                    
                    if (accesLevel.Id == 14)
                    {
                        var allowSchool = await unitOfWork.RoleRepository.ToListAsync(x => x.Id == 14);
                        var arrId = allowSchool.Select(x => x.Id).FirstOrDefault();
                        pred = pred.And(x => x.RoleId == arrId);
                    }
                    else {
                        var lv = await unitOfWork.RoleRepository.ToListAsync(r => r.AccessLevel > accesLevel.AccessLevel);
                        var arr = lv.Select(a => a.Id).ToArray();
                        pred = pred.And(a => arr.Contains(a.RoleId));
                    }
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
                            case "Organization":
                                pred = pred.And(a => a.Organization.Contains(col.Search.Value));
                                break;
                            case "ProjectName":
                                pred = pred.And(a => a.Project.Name.Contains(col.Search.Value));
                                break;
                            case "RoleName":
                                pred = pred.And(a => a.Role.Name.Contains(col.Search.Value));
                                break;
                        }
                    }
                }

                if (dtParameters.Columns.Length > 0)
                {
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        pred = pred.And(a => a.UserName.Contains(searchValue)
                                             || a.Organization.Contains(searchValue)
                                             || (a.Role != null && a.Role.Name.Contains(searchValue))
                                             || (a.Project != null && a.Project.Name.Contains(searchValue))
                        );
                    }
                }

                var data = await unitOfWork.AccountRepository
                    .GetAsync(orderBy: o => o.OrderBy(p => p.Id), pred, true, pageSize, skip,
                        includeProperties: "Project,Role,District,School");

                recordsFiltered = await unitOfWork.AccountRepository.CountAsync(pred);
                recordsTotal = await unitOfWork.AccountRepository.CountAsync(a => true);

                var result = new DtResult<ReadAccountDTO>()
                {
                    Data = _mapper.Map<List<ReadAccountDTO>>(data),
                    Draw = dtParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal
                };

                var json = JsonConvert.SerializeObject(result);

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [ValidateAntiForgeryToken]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Account, Permission = Permissions.Create)]
        public async Task<string> Register(string username, string email, int projectId, int roleId,
            string organization, int id, int schoolId, int contractId = 0, int districtId = 0, int admUnitId = 0)
        {
            try
            {
                contractId = ContractId == 0 ? contractId : ContractId;

                var newAccount =
                    await unitOfWork.AccountRepository.GetSingleAsync(ac =>
                        ac.Id == id || ac.UserName == username);
                if (id < 1 && newAccount != null)
                {
                    return GetText("Account_popup_txt_username_exsist");
                }

                newAccount =
                    await unitOfWork.AccountRepository.GetSingleAsync(ac =>
                        ac.Id == id || ac.Email == email);
                if (id < 1 && newAccount != null)
                {
                    return GetText("Account_popup_txt_email_already_use");
                }

                if (newAccount == null)
                {
                    long uid = RandomGenerator.GenerateRandomNumbers(6);
                    newAccount = new Account();
                    newAccount.Password = string.Empty;
                    newAccount.UserName = username;
                    newAccount.Uid = uid;
                    newAccount.Email = email;
                    newAccount.RoleId = roleId;
                    newAccount.ProjectId = projectId;
                    newAccount.DistrictId = districtId;
                    newAccount.AdministrativeUnitId = admUnitId;
                    newAccount.Organization = organization;
                    newAccount.SchoolId = schoolId;
                    newAccount.ContractId = contractId;
                    await unitOfWork.AccountRepository.AddAsync(newAccount);
                    await SendPassword(email, newAccount);
                }
                else
                {
                    newAccount.UserName = username;
                    newAccount.Email = email;
                    newAccount.RoleId = roleId;
                    newAccount.ContractId = contractId;
                    newAccount.DistrictId = districtId;
                    newAccount.AdministrativeUnitId = admUnitId;
                    newAccount.ProjectId = projectId;
                    newAccount.Organization = organization;
                    newAccount.SchoolId = schoolId;
                    await unitOfWork.SaveAsync();
                }

                return "ok";
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return "failed";
            }
        }

        public async Task<string> ResetPassword(string email)
        {
            try
            {
                var newAccount =
                    await unitOfWork.AccountRepository.GetSingleAsync(ac => ac.Email == email);
                if (newAccount == null)
                {
                    return GetText("Forgetpassword_pass_email_havent_registered");
                }

                await SendPassword(email, newAccount, true);

                return "ok";
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return "failed";
            }
        }

        public async Task<IActionResult> TestEmail(string email, long uid)
        {
            var acc = await unitOfWork.AccountRepository.GetSingleAsync(a => a.Uid == uid);
            await SendPassword(email, acc);

            return Content("OK");
        }

        private async Task SendPassword(string email, Account account, bool reset = false)
        {
            try
            {
                var subjectKey = "SubjectNewPassword";
                var bodeyKey = "BodyNewPassword";
                if (reset)
                {
                    subjectKey = "SubjectResetPassword";
                    bodeyKey = "BodyForgotPassword";
                }

                string baseUrl = _config["UrlChangePassword"];
                var expiryDate = DateTime.Now.AddDays(1).ToString("O");
                string token = String.Format("{0}.{1}", expiryDate, account.Uid);
                string tokenEncode = RandomGenerator.Base64Encode(token);
                string url = String.Format("{0}{1}", baseUrl, tokenEncode);

                var subject = GetText(subjectKey);
                var body = GetText(bodeyKey);
                body = body.Replace("[url]", url);
                body = body.Replace("[user]", account.UserName);

                MailRequest mailRequest = new MailRequest()
                {
                    ToEmail = email,
                    Subject = subject,
                    Body = body
                };
                await _mailService.SendEmailAsync(mailRequest);
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<string> Login(string username, string password)
        {
            try
            {
                var data = await unitOfWork.AccountRepository.GetSingleAsync(ac => ac.UserName == username);
                if (data == null)
                {
                    return "failed";
                }

                var option = new HashingOptions();
                PasswordHasher hasher = new PasswordHasher(new OptionsWrapper<HashingOptions>(option));
                var hash = hasher.Check(data.Password, password);
                if (!hash.Verified)
                {
                    return "failed";
                }

                var claims = new List<Claim>
                {
                    new Claim("Provider", "Userbase"),
                    new Claim("ID", data.Id.ToString()),
                    new Claim(ClaimTypes.Name, data.UserName),
                    new Claim("DisplayName", data.UserName),
                    new Claim(ClaimTypes.Role, data.RoleId.ToString()),
                    new Claim("LastUpdate", ""),
                    new Claim("ProjectId", data.ProjectId?.ToString() ?? ""),
                    new Claim("DistrictId", data.DistrictId?.ToString() ?? ""),
                    new Claim("ContractId", data.ContractId?.ToString() ?? ""),
                    new Claim("SchoolId", data.SchoolId?.ToString() ?? ""),
                    new Claim("AdministrativeUnitId", data.AdministrativeUnitId?.ToString() ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(1),
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return "ok";
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return "failed";
            }
        }


        public IActionResult Signout()
        {
            HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
        
        public static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        public static bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        public static bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        public static bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }
    }
}