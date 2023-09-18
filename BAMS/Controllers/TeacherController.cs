using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Helpers;
using BAMS.Models;
using EightElements.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BAMS.Controllers
{
    public class TeacherController : BaseController
    {
        private IUnitOfWork unitOfWork;
        private IMapper _mapper { get; set; }
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;
        private readonly ITextService _textService;


        public TeacherController(IUnitOfWork unitOfWork,
            IConfiguration config,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
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
                });

                _mapper = configMapper.CreateMapper();
                _mailService = mailService;
            }
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.TeacherAccount, Permission = Permissions.Read)]
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

            var schools = await GetListSchool(ProjectId, DistrictId, SchoolId);
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
            ViewBag.TextTitle = _textService.GetString("Teacher_txt_teacher_accounts", "en");
            return View();
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
                data = data.Prepend(new Project {Id = 0, Name = _textService.GetString("Teacher_form_dropdown_all_project","en") }).ToList();
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
                data = data.Prepend(new Contract {Id = 0, Name = _textService.GetString("Teacher_form_dropdown_all_contracts", "en") }).ToList();
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
                data = data.Prepend(new District {Id = 0, Name = _textService.GetString("Teacher_form_dropdown_all_districts","en") }).ToList();
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

        private async Task<List<School>> GetListSchool(int projectId = 0, int districtId = 0, int schoolId = 0)
        {
            var pred = PredicateBuilder.True<School>();
            if (projectId > 0)
            {
                pred = pred.And(a => a.ProjectId == projectId);
            }

            if (districtId > 0)
            {
                pred = pred.And(a => a.DistrictId == districtId);
            }

            if (schoolId > 0)
            {
                pred = pred.And(a => a.Id == schoolId);
            }

            var data = await unitOfWork.SchoolRepository.GetAsync(predicate: pred,
                orderBy: o => o.OrderBy(a => a.Name));

            if (projectId == 0 && districtId == 0)
            {
                data = data.Prepend(new School {Id = 0, Name = _textService.GetString("Teacher_form_dropdown_empty", "en") }).ToList();
            }

            return data;
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.TeacherList, Permission = Permissions.Read)]
        public async Task<IActionResult> TeacherList(int id = 0) 
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

            var schools = await GetListSchool(ProjectId, DistrictId, SchoolId);
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
            ViewBag.TextTitle = _textService.GetString("Teacher_list_txt_teacher_list", "en");
            return View("index");
        }

    
        public bool getPermission()
        {
            var allow = false;
            if (RoleId == 14) {
                allow = true;
            }
            return allow;
        }



    }
}