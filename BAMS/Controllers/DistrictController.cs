using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAMS.Models;
using BAMS.Helpers;
using BAMS.Data.Models;
using BAMS.Data.Interface;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Security.Claims;
using BAMS.InputValidators;
using Microsoft.Extensions.DependencyInjection;
using EightElements.Services;
using Newtonsoft.Json.Linq;

namespace BAMS.Controllers
{
    public class DistrictController : BaseController
    {
        private IUnitOfWork _uow;
        private IMapper _mapper { get; set; }
        private readonly ILogger<DistrictController> _logger;
        private IChangelogService _changelog;
        private ITextService _textService;
        

        public DistrictController(
            IUnitOfWork unitOfWork,
            ILogger<DistrictController> logger,            
            ITextService textService,
            IChangelogService changelogService
        ) : base(unitOfWork, textService)
        {
            _uow = unitOfWork;
            _logger = logger;
            _changelog = changelogService;
            _textService = textService;
            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CreateDistrictDto, District>();
                    cfg.CreateMap<District, CreateDistrictDto>();
                    cfg.CreateMap<UpdateDistrictDto, District>();
                    cfg.CreateMap<District, UpdateDistrictDto>();
                    cfg.CreateMap<ReadDistrictDto, District>();
                    cfg.CreateMap<District, ReadDistrictDto>();
                    cfg.CreateMap<District, District>();
                    cfg.CreateMap<Account, ReadAccountDTO>();
                    cfg.CreateMap<Project, ReadProjectDto>();
                });

                _mapper = config.CreateMapper();
            }
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District, Permission = Permissions.Read)]
        public async Task<IActionResult> Index(long uid = 0, long projectUid = 0)
        {
            ViewData["ProjectList"] = await GetProjectList();
            ViewData["ProjectUid"] = projectUid > 0 ? projectUid : await _uow.ProjectRepository.GetUidById(ProjectId);
            ViewData["ProjectId"] = ProjectId;
            ViewData["RoleId"] = RoleId;

            if (uid > 0)
            {
                try
                {
                    var district = await _uow.DistrictRepository.GetByUidAsync(uid);
                    bool hasAccess = await HasAccess(district.ProjectId);
                    if (!hasAccess)
                    {
                        //todo: record to security log
                        return new BadRequestObjectResult(new
                            {message = GetText("District_popup_txt_access_violation")});
                    }

                    ViewData["District"] = district;
                    ViewData["ProjectUid"] = await _uow.ProjectRepository.GetUidById(district.ProjectId);
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
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District, 
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetListDistrict(DtParameters dtParameters)
        {
            try
            {
                var searchValue = dtParameters.Search.Value;
                int pageSize = dtParameters.Length;
                int skip = dtParameters.Start;
                int recordsTotal = 0;

                var pred = PredicateBuilder.True<District>();
                if (ProjectId > 0)
                {
                    pred = pred.And(a => a.ProjectId == ProjectId);
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
                            case "PIC":
                                pred = pred.And(a => a.PIC.Contains(col.Search.Value));
                                break;
                            //case "Students":
                            //    pred = pred.And(a => a.Students.ToString().Contains(col.Search.Value));
                            //    break;
                            case "Remarks":
                                pred = pred.And(a => a.Remarks.Contains(col.Search.Value));
                                break;
                        }
                    }
                }

                var colSort = dtParameters.Columns[dtParameters.Order?[0].Column ?? 0];
                var dir = dtParameters.Order?[0].Dir ?? DtOrderDir.Asc;
                Expression<Func<District, dynamic>> sortFunc = a => a.Id;

                switch (colSort.Name)
                {
                    case "Name":
                        sortFunc = a => a.Name;
                        break;
                    case "PICName":
                        sortFunc = a => a.PIC;
                        break;
                    //case "Students":
                    //    sortFunc = a => a.Students;
                    //    break;
                    case "Remarks":
                        sortFunc = a => a.Remarks;
                        break;
                }

                Func<IQueryable<District>, IOrderedQueryable<District>> orderBy = o => o.OrderBy(sortFunc);
                if (dir == DtOrderDir.Desc)
                {
                    orderBy = o => o.OrderByDescending(sortFunc);
                }

                var isCodeAvailable =
                    (await _uow.activationCodeRepository.CountAsync(a =>
                        a.ContractId == ContractId && a.RedeemDate == null)) > 0;

                var data = await _uow.DistrictRepository
                    .GetAsync(
                        selector: a => new ReadDistrictDto()
                        {
                            Id = a.Id,
                            Uid = a.Uid.ToString(),
                            ProjectId = a.ProjectId,
                            ProjectName = a.Project.Name,
                            Name = a.Name,
                            PIC = a.PIC,
                            Schools = a.Schools.Count,
                            Students = a.Schools.Sum(a => a.Students),
                            Remarks = a.Remarks,
                            ActivationCodes = a.ActivationCodes.Count,
                            ActivationCodesActive = a.ActivationCodes.Where(a => a.RedeemDate != null).Count(),
                            //IsCodeAvailable = isCodeAvailable
                        }
                        , orderBy, pred, true, pageSize, skip,
                        includeProperties: "ActivationCodes,Schools,UserAccounts,Project");
                var recordsFiltered = await _uow.DistrictRepository.CountAsync(pred);
                recordsTotal = await _uow.DistrictRepository.CountAsync(pred);

                var result = new DtResult<ReadDistrictDto>()
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


        [Route("get-district/{uid}")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetDistrict(long uid)
        {
            var district = await _uow.DistrictRepository.GetByUidAsync(uid);
            if (district == null)
            {
                return Failed(GetText("District_popup_txt_district_not_found"));
            }

            var dto = new
            {
                uid = district.Uid,
                projectUid = await _uow.ProjectRepository.GetUidById(district.ProjectId),
                name = district.Name,
                picName = district.PIC,
                remarks = district.Remarks
            };

            return Success("district", JObject.FromObject(dto));
        }


        [Route("new-district")]
        [Route("district/{uid}")]
        [Route("district/detail")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Read)]
        public async Task<IActionResult> Detail(long uid = 0, long projectUid = 0)
        {
            ViewData["ProjectList"] = await GetProjectList();
            ViewData["ProjectUid"] = projectUid;

            if (uid > 0)
            {
                try
                {
                    var district = await _uow.DistrictRepository.GetByUidAsync(uid);
                    bool hasAccess = await HasAccess(district.ProjectId);
                    if (!hasAccess)
                    {
                        //todo: record to security log
                        var message = GetText("District_popup_txt_access_violation");
                        return Failed(message);
                    }

                    ViewData["District"] = district;
                    ViewData["ProjectUid"] = await _uow.ProjectRepository.GetUidById(district.ProjectId);
                }
                catch (Exception e)
                {
                    _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                    return new BadRequestObjectResult(new {message = e.Message});
                }
            }

            return View("CreateUpdate");
        }

        private async Task<bool> HasAccess(int projectId)
        {
            var user = await _uow.AccountRepository.GetByIdAsync(UserId);

            if (user.ProjectId == 0)
            {
                return true; // super-user
            }

            if (user.ProjectId == projectId)
            {
                //user is project admin (can manage districts)
                return true;
            }

            return false;
        }

        [Route("district/create")]
        [Route("create-district")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Create)]
        public async Task<IActionResult> Create([FromBody] CreateDistrictDto dto)
        {
            try
            {
                var validationResults =
                    await DistrictValidator.ValidateCreateDistrict(UserId, dto, _uow,_textService,GetClientLanguage());

                if (validationResults.Count > 0)
                {
                    string message = string.Join("<br/>", validationResults);
                    return Failed(message);
                }

                var district = _mapper.Map<District>(dto);
                district.Uid = await _uow.DistrictRepository.GenerateUid();
                district.ProjectId = await _uow.ProjectRepository.GetIdByUid(dto.ProjectUid);
                district.CreateDate = DateTime.Now;
                district.CreatedBy = UserId;

                await _uow.DistrictRepository.AddAsync(district);

                return Success();
            }
            catch (Exception e)
            {
                //todo: move error logger to BaseController
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return Error(e.Message);
            }
        }


        [Route("district/update")]
        [Route("update-district")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateDistrictDto dto)
        {
            bool hasAccess = await HasAccess(ProjectId);
            if (!hasAccess)
            {
                //todo: record to security log
                var message = GetText("District_popup_txt_access_violation");
                return Failed(message);
            }

            try
            {
                var district = await _uow.DistrictRepository.GetByUidAsync(dto.Uid);
                if (district == null)
                {
                    return Failed(GetText("District_popup_txt_district_not_found"));
                }

                var validationResults = await DistrictValidator.ValidateUpdateDistrict(
                    UserId, district.Id, dto, _uow,_textService, GetClientLanguage());

                if (validationResults.Count > 0)
                {
                    string message = string.Join("<br/>", validationResults);
                    return Failed(message);
                }

                string oldValue = JsonConvert.SerializeObject(district);

                district.ProjectId = await _uow.ProjectRepository.GetIdByUid(dto.ProjectUid);
                district.Name = dto.Name;
                district.PIC = dto.PIC;
                district.Remarks = dto.Remarks;
                await _uow.DistrictRepository.UpdateAsync(district);

                string newValue = JsonConvert.SerializeObject(district);
                await _changelog.Log("District", district.Id, UserId, oldValue, newValue);

                return Success();
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return Error(e.Message);
            }
        }


        [Route("district/delete/{uid}")]
        [Route("delete-district/{uid}")]
        [HttpGet]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District,
            Permission = Permissions.Delete)]
        public async Task<IActionResult> Delete(long uid)
        {
            try
            {
                var district = await _uow.DistrictRepository.GetByUidAsync(uid);
                if (district == null)
                {
                    var message = GetText("District_popup_txt_district_not_found");
                    return Failed(message);
                }

                var school = await _uow.SchoolRepository.CountAsync(s => s.DistrictId == district.Id);
                if (school > 0)
                {
                    var message = GetText("District_popup_txt_cannot_delete");
                    return Failed(message);
                }

                district.DeleteDate = DateTime.Now;
                district.DeletedBy = UserId;
                await _uow.DistrictRepository.UpdateAsync(district);

                return Success();
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return Error(e.Message);
            }
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District, Permission = Permissions.Read)]
        public async Task<IActionResult> GetProjects()
        {
            try
            {
                var projects = await GetProjectList();
                var dto = _mapper.Map<List<ReadProjectDto>>(projects);

                var json = JsonConvert.SerializeObject(new {message = "success", data = dto, status = 0});

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(new {message = e.Message});
            }
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadDistricts(string rawXls, int projectId = 0)
        {
            var data = JsonConvert.DeserializeObject<List<UploadDistrictDTO>>(rawXls);
            var validationResults = new List<string>();
            var uploadDistrict = new List<District>();
            foreach (var upload in data)
            {
                if (string.IsNullOrEmpty(upload.DistrictName) ||
                    string.IsNullOrEmpty(upload.PIC))
                {
                    validationResults.Add(GetText("District_popup_txt_incorrect_format"));
                    break;
                }

                if (upload.DistrictName.Length < 6)
                {
                    validationResults.Add(GetText("District_popup_txt_invalid_district_name"));
                    break;
                }
                
                var district =
                    await _uow.DistrictRepository.GetSingleAsync(val =>
                        val.ProjectId == ProjectId && val.Name == upload.DistrictName);
                if (projectId < 1)
                {
                    validationResults.Add(GetText("District_popup_txt_project_id_null"));
                    break;
                }

                if (district == null)
                {
                    district = new District();
                    district.Name = upload.DistrictName;
                    district.PIC = upload.PIC;
                    district.Uid = await _uow.DistrictRepository.GenerateUid();
                    district.ProjectId = projectId;
                    district.CreateDate = DateTime.Now;
                    district.CreatedBy = UserId;
                    uploadDistrict.Add(district);
                }
                else
                {
                    validationResults.Add(GetText("District_popup_txt_district_exists"));
                }
            }

            if (validationResults.Count > 0)
            {
                var errorJson =
                    JsonConvert.SerializeObject(new {message = string.Join("\n", validationResults)});
                return Content(errorJson, "application/json");
            }
            
            await _uow.DistrictRepository.AddRangeAsync(uploadDistrict);
            await _uow.SaveAsync();

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

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.District, Permission = Permissions.Update)]
        public async Task<IActionResult> AllocateCode(long districtUid, int qty,
            [FromServices] IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                var district = await _uow.DistrictRepository.GetByUidAsync(districtUid);
                if (district == null)
                {
                    return new NotFoundObjectResult(new {message = GetText("District_popup_txt_data_not_found")});
                }

                bool hasAccess = await HasAccess(district.ProjectId);
                if (!hasAccess)
                {
                    //todo: record to security log
                    return new BadRequestObjectResult(new
                        {message = GetText("District_popup_txt_access_violation")});
                }

                var contract = (await _uow.ContractRepository
                        .GetAsync(predicate: a => a.ProjectId == district.ProjectId && a.Id == ContractId,
                            includeProperties: "ActivationCodeRequest"))
                    .SingleOrDefault();
                if (contract == null)
                {
                    return new NotFoundObjectResult(new {message = GetText("District_popup_txt_contract_not_found")});
                }

                if (contract.ActivationCodeRequest == null)
                {
                    return new NotFoundObjectResult(new
                        {message = GetText("District_popup_txt_activation_not_available")});
                }

                if (contract.ActivationCodes != contract.ActivationCodeRequest.AmountCodes)
                {
                    return new NotFoundObjectResult(new
                        {message = GetText("District_popup_txt_activation_not_available")});
                }
                //if(qty > district.Students)
                //{
                //    return new BadRequestObjectResult(new { message = "Requested amount not valid" });
                //}

                var districtCodesQty = await _uow.activationCodeRepository
                    .CountAsync(a =>
                        a.ProjectId == contract.ProjectId && a.ContractId == contract.Id &&
                        a.DistrictId == district.Id);

                qty = qty - districtCodesQty;

                if (qty <= 0)
                {
                    return new BadRequestObjectResult(new
                        {message = GetText("District_popup_txt_request_amount_not_valid")});
                }

                var codesQty = await _uow.activationCodeRepository
                    .CountAsync(a =>
                        a.ProjectId == contract.ProjectId && a.ContractId == contract.Id && a.DistrictId == 0 &&
                        a.SchoolId == 0 && a.UserAccountId == 0);
                if (codesQty < qty)
                {
                    return new NotFoundObjectResult(new {message = GetText("District_popup_txt_request_rejected")});
                }

                _ = Task.Run(async () =>
                {
                    try
                    {
                        var districtId = district.Id;
                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                            var codes = await uow.activationCodeRepository
                                .GetAsync(
                                    predicate: a =>
                                        a.ProjectId == contract.ProjectId && a.ContractId == contract.Id &&
                                        a.DistrictId == 0 && a.SchoolId == 0 && a.UserAccountId == 0,
                                    usePaging: true, pageSize: qty);
                            foreach (var code in codes)
                            {
                                code.DistrictId = districtId;
                            }

                            await uow.SaveAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"error while allocate code district | {e.Message}");
                    }
                });

                return new OkObjectResult(new {status = 0, message = GetText("District_popup_txt_processing")});
            }
            catch (Exception e)
            {
                _logger.LogError($"error while allocate code district | {e.Message}");
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}