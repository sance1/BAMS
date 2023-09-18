using AutoMapper;
using Bams.Workflows;
using Bams.Workflows.Enums;
using Bams.Workflows.InputValidators;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Helpers;
using BAMS.Models;
using BAMS.Workflows.Models;
using EightElements.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BAMS.Controllers
{

    public class ProjectController : BaseController
    {
        private IProjectWorkflow _workflow;
        private IProjectValidator _validator;
        private IUnitOfWork _uow;
        private IMapper _mapper { get; set; }
        private readonly ILogger<ProjectController> _logger;
        

        public ProjectController(
            IProjectWorkflow workflow,
            IProjectValidator validator,
            IUnitOfWork unitOfWork,             
            ITextService textService,
            IChangelogService changelogService,
            ILogger<ProjectController> logger
            ) : base(unitOfWork, textService)
        {
            _workflow = workflow;
            _validator = validator;
            _uow = unitOfWork;
            _logger = logger;            

            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ReadProjectDto, Project>()                    
                    .ForMember(dest => dest.Country, act => act.MapFrom(src => src.CountryName))                 
                    .ForMember(dest => dest.Province, act => act.MapFrom(src => src.ProvinceName));
                    cfg.CreateMap<Project, ReadProjectDto>();
                    cfg.CreateMap<Project, Project>();
                });

                _mapper = config.CreateMapper();
            }
        }

        [Route("projects")]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Project, Permission = Permissions.Read)]
        public async Task<IActionResult> Index(long uid = 0)
        {
            ViewBag.Countries = await _uow.countryRepository.GetAsync();
            if (uid > 0)
            {
                try
                {                     
                    var project = await _uow.ProjectRepository.GetByUidAsync(uid);
                    ViewData["Project"] = project;
                }
                catch (Exception e)
                {
                    _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                    return new BadRequestObjectResult(new { message = e.Message });
                }

            }

            return View();
        }

        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Project, 
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetListProject(DtParameters dtParameters)
        {
            try
            {
                
                var searchValue = dtParameters.Search.Value;
                int pageSize = dtParameters.Length;
                int skip = dtParameters.Start;
                int recordsFiltered = 0;
                int recordsTotal = 0;

                var pred = PredicateBuilder.True<Project>();
                foreach(var col in dtParameters.Columns)
                {
                    if (!string.IsNullOrEmpty(col.Search.Value))
                    {
                        switch (col.Name)
                        {
                            case "Name":
                                pred = pred.And(a => a.Name.Contains(col.Search.Value));
                                break;
                            case "ContactPerson":
                                pred = pred.And(a => a.ContactPerson.Contains(col.Search.Value));
                                break;
                            case "PartnerName":
                                pred = pred.And(a => a.PartnerName.Contains(col.Search.Value));
                                break;
                            case "PartnerPIC":
                                pred = pred.And(a => a.PartnerPIC.Contains(col.Search.Value));
                                break;
                            case "Remarks":
                                pred = pred.And(a => a.Remarks.Contains(col.Search.Value));
                                break;
                            case "Contracts":
                                pred = pred.And(a => a.Contracts.Count.ToString().Contains(col.Search.Value));
                                break;
                            case "Districts":
                                pred = pred.And(a => a.Districts.Count.ToString().Contains(col.Search.Value));
                                break;
                            case "Schools":
                                pred = pred.And(a => a.Schools.Count.ToString().Contains(col.Search.Value));
                                break;
                        }
                    }
                }
                
                var colSort = dtParameters.Columns[dtParameters.Order[0].Column];
                var dir = dtParameters.Order[0].Dir;
                Expression<Func<Project, dynamic>> sortFunc = a => a.Id;

                switch (colSort.Name)
                {
                    case "Name":
                        sortFunc = a => a.Name;
                        break;
                    case "PartnerName":
                        sortFunc = a => a.PartnerName;
                        break;
                    case "ContactPerson":
                        sortFunc = a => a.ContactPerson;
                        break;
                    case "PartnerPIC":
                        sortFunc = a => a.PartnerPIC;
                        break;
                    case "Remarks":
                        sortFunc = a => a.Remarks;
                        break;
                    case "Contracts":
                        sortFunc = a => a.Contracts.Count;
                        break;
                    case "Districts":
                        sortFunc = a => a.Districts.Count;
                        break;
                    case "Schools":
                        sortFunc = a => a.Schools.Count;
                        break;
                }

                Func<IQueryable<Project>, IOrderedQueryable<Project>> orderBy = o => o.OrderBy(sortFunc);
                if(dir == DtOrderDir.Desc)
                {
                    orderBy = o => o.OrderByDescending(sortFunc);
                }

                var data = await _uow.ProjectRepository
                    .GetAsync(
                        selector: a => new ReadProjectDto()
                        {
                            Uid = a.Uid,
                            Id = a.Id,
                            Name = a.Name,
                            ContactPerson = a.ContactPerson,
                            PartnerName = a.PartnerName,
                            PartnerPIC = a.PartnerPIC,
                            Remarks = a.Remarks,
                            Contracts = a.Contracts.Count,
                            Districts = a.Districts.Count,
                            Schools = a.Schools.Count
                        }
                        ,orderBy, pred, true, pageSize, skip, includeProperties: "Contracts,Districts,Schools");
                recordsFiltered = await _uow.ProjectRepository.CountAsync(pred);
                recordsTotal = await _uow.ProjectRepository.CountAsync(a => true);

                var result = new DtResult<ReadProjectDto>()
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
                _logger.LogError($"error while get data | {e.Message}");
                return new BadRequestObjectResult(e.Message);
            }
        }



        [Route("get-project-list")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Project,
            Permission = Permissions.Create)]
        public async Task<IActionResult> GetProjectList()
        {
            var projects = await _uow.ProjectRepository.GetAsync(
                selector: p => new { uid = p.Uid, name = p.Name },
                predicate: p => p.DeleteDate == null);

            return Success("projects", JArray.FromObject(projects));
        }


        [Route("get-project/{uid}")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Project,
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetProject(long uid)
        {

            var getProject = await _uow.ProjectRepository.GetByUidAsync(uid);

            if (getProject !=null && getProject.CountryId != null) {
                var getCountry = await _uow.countryRepository.GetByIdAsync(getProject.CountryId.Value);
                getProject.Country.Name = getCountry.Name;
            }
            if (getProject != null && getProject.ProvinceId != null)
            {
                var getProvince = await _uow.provinceRepository.GetByIdAsync(getProject.ProvinceId.Value);
                getProject.Province.Name = getProvince.Name;     
            }

            /*var project = await _uow.ProjectRepository.GetByUidAsync(
            uid, p => new
            {
                uid = p.Uid,
                name = p.Name,
                partnerName = p.PartnerName,
                contactPerson = p.ContactPerson,
                partnerPIC = p.PartnerPIC,
                remarks = p.Remarks,
                countryId = getProject.CountryId,
                provinceId = getProject.ProvinceId,
                contryName = p.Country?,
                provinceName = provinceName
            });*/
            var project = _mapper.Map<ReadProjectDto>(getProject);

            if (project == null)
            {
                string message = GetText("Project_popup_project_not_found");
                return Failed(message);
            }
            

            return Success("project", JObject.FromObject(project));
        }



        [Route("projects/create")]
        [Route("create-project")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Project, 
            Permission = Permissions.Create)]
        public async Task<IActionResult> Create([FromBody] ProjectDto dto)
        {            
            var b = dto.Name;
            var splitName = b.Split('-');
            
            var getCountryByCountryId = await _uow.countryRepository.GetByIdAsync(int.Parse(splitName[0]));
            var getProvinceByProvinceId = await _uow.provinceRepository.GetByIdAsync(int.Parse(splitName[1]));
            dto.Name = getCountryByCountryId.Code + "-" + getProvinceByProvinceId.Code;

            var validationResults = await _validator.ValidateCreate(dto);

            if (validationResults.Count > 0)
            {
                string message = string.Join("<br/>", validationResults);
                return Failed(message);
            }

            var result = await _workflow.CreateProject(UserId, dto);
            switch(result)
            {
                case WorkflowResult.Success:
                    return Success();
                default:
                    return Failed(GetText("Project_popup_unknow_error"));
            }
        }

        [Route("project/update")]
        [Route("update-project")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Project, 
            Permission = Permissions.Update)]
        public async Task<IActionResult> Update([FromBody] ProjectDto dto)
        {
            var b = dto.Name;
            var splitName = b.Split('-');

            var getCountryByCountryId = await _uow.countryRepository.GetByIdAsync(int.Parse(splitName[0]));
            var getProvinceByProvinceId = await _uow.provinceRepository.GetByIdAsync(int.Parse(splitName[1]));
            dto.Name = getCountryByCountryId.Code + "-" + getProvinceByProvinceId.Code;

            var validationResults = await _validator.ValidateUpdate(dto,GetClientLanguage());

            if (validationResults.Count > 0)
            {
                return Failed(string.Join("<br/>", validationResults));
            }

            var result = await _workflow.UpdateProject(UserId, dto);
            switch(result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:                        
                    return Failed(GetText("Project_popup_project_not_found"));
                default:
                    return Error(GetText("Project_popup_unknow_error"));
            }

        }
        
        [HttpGet]
        [Route("delete-project/{uid}")]
        [Route("project/delete/{uid}")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Project, 
            Permission = Permissions.Delete)]
        public async Task<IActionResult> Delete(long uid)
        {
            var result = await _workflow.DeleteProject(UserId, uid);

            switch(result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:                    
                    return Failed(GetText("Project_popup_project_not_found"));
                case WorkflowResult.ActionProhibited:                    
                    return Failed(GetText("Project_popup_cannot_delete_project"));
                default:
                    return Error(GetText("Project_popup_unknow_error"));
            }

        }

        [Route("/Province/GetByCountryId/{countryId}")]
        public async Task<IActionResult> GetByCountryId(int countryId)
        {            
            var list = await _uow.provinceRepository.GetAsync(
                predicate: u => u.CountryId == countryId
                );            

            return Success("province", JArray.FromObject(list));
        }

        [Route("/Level/GetByCountryCode/{countryCode}")]
        public async Task<IActionResult> GetByCountryCode(string countryCode)
        {
            var list = await _uow.administrativeLevelRepository.GetAsync(
                predicate: u => u.Id == 1
                );

            return Success("level",JArray.FromObject(list));
        }

    }
}
