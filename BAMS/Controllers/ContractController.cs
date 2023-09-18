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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BAMS.Controllers
{
    public class ContractController : BaseController
    {
        private IContractWorkflow _workflow;
        private IContractValidator _validator;
        private IUnitOfWork _uow;
        private IMapper _mapper { get; set; }
        private readonly IConfiguration _config;
        private readonly ILogger<ContractController> _logger;
        private IChangelogService _changelog;
        private ITextService _textService;

        public ContractController(
            IContractWorkflow workflow,
            IContractValidator validator,
            IUnitOfWork unitOfWork,
            IConfiguration config,
            ILogger<ContractController> logger,
            ITextService textService,
            IChangelogService changelogService
            ) : base(unitOfWork, textService)
        {
            _workflow = workflow;
            _validator = validator;
            _uow = unitOfWork;
            _textService = textService;            
            _config = config;
            _logger = logger;
            _changelog = changelogService;

            if (_mapper == null)
            {
                var mapperConfig = new MapperConfiguration(cfg =>
                {                    
                    cfg.CreateMap<ReadContractDto, Contract>();
                    cfg.CreateMap<Contract, ReadContractDto>()
                        .ForMember(dest => dest.DeletedDate,
                            act => act.MapFrom(src => src.DeleteDate))
                        .ForMember(dest => dest.Uid, act => act.MapFrom(src => src.Uid.ToString()))
                        .ForMember(dest => dest.ProjectName, act => act.MapFrom(src => src.Project.Name.ToString()))
                        .ForMember(dest => dest.UploadedCode,
                            act => act.MapFrom(src => src.ActivationCodeRequest.AmountCodes))
                        .ForMember(dest => dest.CodeRequestDate, opt => opt.NullSubstitute(null))
                        .ForMember(dest => dest.CodeRequestDate,
                            act => act.MapFrom(src => src.ActivationCodeRequest.CreateDate))
                        .ForMember(dest => dest.CodeUploadDate, opt => opt.NullSubstitute(null))
                        .ForMember(dest => dest.CodeUploadDate,
                            act => act.MapFrom(src => src.ActivationCodeRequest.ActivationCodeUpload.CreateDate))
                        .ForMember(dest => dest.RemarksRequest,
                            act => act.MapFrom(src => src.ActivationCodeRequest.Remarks))
                        .ForMember(dest => dest.StatusText,
                            act => act.MapFrom(src => src.Status == 0 ? textService.GetString("Contract_popup_active","en") : src.Status == 1 ? textService.GetString("Contract_popup_fullfilled","en") : textService.GetString("Contract_popup_canceled","en")))
                        .ForMember(dest => dest.StatusUpload, act => act.MapFrom(src => src.ActivationCodeRequest.StatusUpload))
                        .ForMember(dest => dest.StatusUploadText,
                            act => act.MapFrom(src => src.ActivationCodeRequest.StatusUpload == 0 ? textService.GetString("Contract_popup_need_to_upload_code","en") 
                            : src.ActivationCodeRequest.StatusUpload == 1 ? textService.GetString("Contract_popup_uploaded_successfully","en") 
                            : src.ActivationCodeRequest.StatusUpload == 2 ? textService.GetString("Contract_popup_code_not_enough","en") 
                            : src.ActivationCodeRequest.StatusUpload == 3 ? textService.GetString("Contract_popup_code_too_many","en")
                            : src.ActivationCodeRequest.StatusUpload == 4 ? textService.GetString("Contract_popup_upload_error_duplicate_codes","en")
                            : "-"));
                    cfg.CreateMap<Contract, Contract>();
                    cfg.CreateMap<CreateActivationCodeRequestDTO, ActivationCodeRequest>();
                    cfg.CreateMap<ActivationCodeRequest, CreateActivationCodeRequestDTO>();
                    cfg.CreateMap<ActivationCodeRequest, ActivationCodeRequest>();
                    cfg.CreateMap<UpdateActivationCodeRequestDTO, ActivationCodeRequest>(MemberList.Source);
                    cfg.CreateMap<ActivationCodeRequest, UpdateActivationCodeRequestDTO>();
                });

                _mapper = mapperConfig.CreateMapper();
            }
        }

        [Route("contract/{projectId}")]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Contract, Permission = Permissions.Read)]
        public async Task<IActionResult> Index(int projectId, long projectUid = 0, long contractUid = 0,
            string page = "")
        {
            var project = await _uow.ProjectRepository.GetSingleAsync(a => a.Id == projectId);
            if (project == null)
            {
                return View("Views/Error/NotFound.cshtml");
            }

            var projectList = await _uow.ProjectRepository.GetAsync(
                predicate: p => p.DeleteDate == null);
            ViewData["ProjectList"] = projectList;

            ViewData["Contract"] = null;
            ViewData["ContractUid"] = contractUid;
            Contract contract = null;

            if (contractUid > 0)
            {
                try
                {
                    contract = await _uow.ContractRepository.GetByUidAsync(contractUid);
                    ViewData["Contract"] = contract;
                    ViewData["ProjectUid"] = await _uow.ProjectRepository.GetUidById(contract.ProjectId);
                }
                catch (Exception e)
                {
                    _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                    ViewBag.Message = e.Message;
                    return View("Views/Error/Index.cshtml");
                }
            }

            ActivationCodeRequest acReq = null;

            if (page == "activatiocoderequest" && contractUid > 0 && contract != null)
            {
                acReq = await _uow.activationCodeRequestRepository.GetSingleAsync(a => a.ContractId == contract.Id);
            }


            ViewData["project"] = project;
            ViewData["Contract"] = contract;
            ViewData["projectId"] = projectId;
            ViewData["contractId"] = contract?.Id ?? 0;
            ViewData["acReq"] = acReq;
            ViewData["page"] = page;

            ViewData["projectUid"] = projectUid > 0 ? projectUid : project.Uid;
            ViewData["projectName"] = project.Name;
            return View();
        }

        [Route("contract/all")]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Contract, Permission = Permissions.Read)]
        public async Task<IActionResult> AllContract(long contractUid = 0)
        {
            try
            {
                ViewData["contract"] = null;
                ViewData["acReq"] = null;
                if (contractUid > 0)
                {
                    var acReq = await _uow.GetContractWithActivationCodeRequest(contractUid);
                    if (acReq == null)
                    {
                        _logger.LogError($"contract uid [{contractUid}] not found");
                    }

                    if (acReq.Contract == null)
                    {
                        _logger.LogError($"contract uid [{contractUid}] not found");
                    }

                    ViewData["contract"] = acReq.Contract;
                    ViewData["acReq"] = acReq;
                }
            }
            catch (Exception e)
            {
            }

            return View("AllContract");
        }

        [HttpPost]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Contract, Permission = Permissions.Read)]
        public async Task<IActionResult> GetListContract(int projectId, [FromForm] DtParameters dtParameters, bool ignoreFilter = false)
        {
            try
            {
                var searchValue = dtParameters.Search.Value;
                int pageSize = dtParameters.Length;
                int skip = dtParameters.Start;
                int recordsFiltered = 0;
                int recordsTotal = 0;

                var pred = PredicateBuilder.True<Contract>();
                if (projectId > 0)
                {
                    pred = pred.And(a => a.ProjectId == projectId);
                }
                else
                {
                    pred = pred.And(a => a.ActivationCodeRequest != null);
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
                            case "StartDate":
                                pred = pred.And(a => a.StartDate.ToString().Contains(col.Search.Value));
                                break;
                            case "EndDate":
                                pred = pred.And(a => a.EndDate.ToString().Contains(col.Search.Value));
                                break;
                            case "Students":
                                pred = pred.And(a => a.Students.ToString().Contains(col.Search.Value));
                                break;
                            case "ActivationCodes":
                                pred = pred.And(a => a.ActivationCodes.ToString().Contains(col.Search.Value));
                                break;
                            case "Status":
                                pred = pred.And(a => a.Status.ToString() == col.Search.Value);
                                break;
                            case "Remarks":
                                pred = pred.And(a => a.Remarks.Contains(col.Search.Value));
                                break;
                            case "UploadedCode":
                                pred = pred.And(a =>
                                    a.ActivationCodeRequest.AmountCodes.ToString().Contains(col.Search.Value));
                                break;
                            case "CodeRequestDate":
                                pred = pred.And(a =>
                                    a.ActivationCodeRequest.CreateDate.ToString().Contains(col.Search.Value));
                                break;
                            case "CodeUploadDate":
                                pred = pred.And(a =>
                                    a.ActivationCodeRequest.ActivationCodeUpload.CreateDate.ToString().Contains(col.Search.Value));
                                break;
                        }
                    }
                }

                var colSort = dtParameters.Columns[dtParameters.Order[0].Column];
                var dir = dtParameters.Order[0].Dir;
                Expression<Func<Contract, dynamic>> sortFunc = a => a.Id;

                switch (colSort.Name)
                {
                    case "Name":
                        sortFunc = a => a.Name;
                        break;
                    case "StartDate":
                        sortFunc = a => a.StartDate;
                        break;
                    case "EndDate":
                        sortFunc = a => a.EndDate;
                        break;
                    case "Students":
                        sortFunc = a => a.Students;
                        break;
                    case "ActivationCodes":
                        sortFunc = a => a.ActivationCodes;
                        break;
                    case "Status":
                        sortFunc = a => a.Status;
                        break;
                    case "Remarks":
                        sortFunc = a => a.Remarks;
                        break;
                    case "UploadedCode":
                        sortFunc = a => a.ActivationCodeRequest.AmountCodes;
                        break;
                    case "CodeRequestDate":
                        sortFunc = a => a.ActivationCodeRequest.CreateDate;
                        break;
                    case "CodeUploadDate":
                        sortFunc = a => a.ActivationCodeRequest.ActivationCodeUpload.CreateDate;
                        break;
                }

                Func<IQueryable<Contract>, IOrderedQueryable<Contract>> orderBy = o => o.OrderBy(sortFunc);
                if (dir == DtOrderDir.Desc)
                {
                    orderBy = o => o.OrderByDescending(sortFunc);
                }

                var data = await _uow.GetContractsIncludeAsync(orderBy, pred, pageSize, skip, ignoreFilter);

                recordsFiltered = await _uow.ContractRepository.CountAsync(pred);
                recordsTotal = await _uow.ContractRepository.CountAsync(a => true);

                var result = new DtResult<ReadContractDto>()
                {
                    Data = _mapper.Map<List<ReadContractDto>>(data),
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
                return new BadRequestObjectResult(new {message = e.Message});
            }
        }


        [Route("get-contract/{uid}")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Contract,
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetContract(long uid)
        {
            var contract = await _uow.ContractRepository.GetByUidAsync(uid);
            if (contract == null)
            {
                return Failed(GetText("Contract_popup_txt_contract_not_found"));
            }

            var requestFound = await _uow.activationCodeRequestRepository.CountAsync(r =>
                r.ContractId == contract.Id);

            var dto = new
            {
                uid = contract.Uid,
                projectUid = await _uow.ProjectRepository.GetUidById(contract.ProjectId),
                name = contract.Name,
                startDate = contract.StartDate.ToString("yyyy-MM-dd"),
                endDate = contract.EndDate.ToString("yyyy-MM-dd"),
                activationCodes = contract.ActivationCodes,
                remarks = contract.Remarks,
                readOnly = requestFound > 0
            };

            return Success("contract", JObject.FromObject(dto));
        }


        //Deprecated
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Contract,
            Permission = Permissions.Read)]
        //[Route("contract/{projectId}/detail/{contractId}")]
        [Route("new-contract")]
        [Route("new-contract/{projectId}")]
        [Route("contract/detail/{contractUid}")]
        [Route("contract/detail")]
        public async Task<IActionResult> Detail(long contractUid = 0, long projectUid = 0, int projectId = 0)
        {
            var projectList = await _uow.ProjectRepository.GetAsync(
                predicate: p => p.DeleteDate == null);
            ViewData["ProjectList"] = projectList;
            ViewData["ProjectUid"] = projectUid;
            if (projectId > 0)
            {
                var project = await _uow.ProjectRepository.GetSingleAsync(p => p.Id == projectId);
                ViewData["ProjectUid"] = project.Uid;
            }

            if (contractUid != 0)
            {
                try
                {
                    var contract = await _uow.ContractRepository.GetByUidAsync(contractUid);
                    ViewData["Contract"] = contract;
                    ViewData["ProjectUid"] = await _uow.ProjectRepository.GetUidById(contract.ProjectId);
                }
                catch (Exception e)
                {
                    _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                    ViewBag.Message = e.Message;
                    return View("Views/Error/Index.cshtml");
                }
            }

            return View("CreateUpdate");
        }


        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Contract,
            Permission = Permissions.Create)]
        [Route("create-contract")]
        public async Task<IActionResult> Create([FromBody] ContractDto dto)
        {
            var countContract = (await _uow.ContractRepository.CountAsync() + 1).ToString();
            countContract = countContract.PadLeft(3, '0');
            var dt = dto.StartDate;
            var project = await _uow.ProjectRepository.GetByUidAsync(dto.ProjectUid);
            var name = dto.Name;
            dto.Name = countContract + "/" + dt.Month.ToString() + "/" + dt.Year.ToString() + "/" + project.Name + "-" + name;

            var validationResults = await _validator.ValidateCreate(dto,GetClientLanguage());
            if (validationResults.Count > 0)
            {
                string message = string.Join("<br/>", validationResults);
                return Failed(message);
            }

            var result = await _workflow.CreateContract(UserId, dto);
            switch(result)
            {
                case WorkflowResult.Success:
                    return Success();
                default:
                    return Failed(GetText("Contract_popup_txt_unknow_error"));
            }
        }


        [Route("update-contract")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Contract,
            Permission = Permissions.Update)]
        public async Task<IActionResult> Update([FromBody] ContractDto dto)
        {
            var validationResults = await _validator.ValidateUpdate(dto,GetClientLanguage());
            if (validationResults.Count > 0)
            {
                return Failed(string.Join("<br/>", validationResults));
            }

            var result = await _workflow.UpdateContract(UserId, dto);
            switch(result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:
                    return Failed(GetText("Contract_popup_txt_data_not_found"));
                case WorkflowResult.ActionProhibited:
                    return Failed(GetText("Contract_popup_txt_activation_code_has_been_send"));
                default:
                    return Error(GetText("Contract_popup_txt_unknow_error"));
            }
        }


        [HttpGet]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Contract,
            Permission = Permissions.Delete)]
        [Route("delete-contract/{uid}")]
        public async Task<IActionResult> Delete(long uid)
        {
            var result = await _workflow.DeleteContract(UserId, uid);

            switch(result)
            {
                case WorkflowResult.Success:
                    return Success();
                case WorkflowResult.DataNotFound:
                    return Failed(GetText("Contract_popup_txt_data_not_found"));
                case WorkflowResult.ActionProhibited:
                    return Failed(GetText("Contract_popup_txt_has_been_uploaded"));
                default:
                    return Error(GetText("Contract_popup_txt_unknow_error"));
            }
        }

        [Route("contract/{contractUid}/request-activation-code/{requestUid}")]
        [HttpGet]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.ActivationCodeRequest, Permission = Permissions.Create)]
        public async Task<IActionResult> CreateACRequest(long contractUid, long requestUid = 0)
        {
            try
            {
                var contract = await _uow.ContractRepository.GetByUidAsync(contractUid);
                if (contract == null)
                {
                    return View("Views/Error/NotFound.cshtml");
                }

                var project = await _uow.ProjectRepository.GetByIdAsync(contract.Id);

                var acReq = await _uow.activationCodeRequestRepository.GetSingleAsync(a => a.ContractId == contract.Id);

                ViewData["project"] = project;
                ViewData["contract"] = contract;
                ViewData["projectId"] = project.Id;
                ViewData["contractId"] = contract.Id;
                ViewData["acReq"] = acReq;

                return View("CreateUpdateACReq");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(new {message = e.Message});
            }
        }

        [HttpPost]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.ActivationCodeRequest, Permission = Permissions.Create)]
        public async Task<IActionResult> CreateACRequest([FromBody] CreateActivationCodeRequestDTO dto)
        {
            try
            {
                var isExist = await _uow.activationCodeRequestRepository.GetSingleAsync(a =>
                    a.ProjectId == dto.ProjectId && a.ContractId == dto.ContractId);
                if (isExist != null)
                {
                    return new BadRequestObjectResult(new {message = GetText("Contract_popup_txt_request_already_exist") });
                }

                var data = _mapper.Map<ActivationCodeRequest>(dto);
                data.CreateDate = DateTime.Now;
                data.AccountId = int.Parse(ControllerContext.HttpContext.User.Claims.Where(a => a.Type == "ID")
                    .SingleOrDefault().Value);
                data.CreatedBy = data.AccountId;

                data = await _uow.activationCodeRequestRepository.AddAsync(data);
                

                var json = JsonConvert.SerializeObject(new {status = 0, message = GetText("Contract_popup_txt_successfully_submitted")});

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(new {message = e.Message});
            }
        }

        [HttpPost]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.ActivationCodeRequest, Permission = Permissions.Update)]
        public async Task<IActionResult> UpdateACRequest([FromBody] UpdateActivationCodeRequestDTO dto)
        {
            try
            {
                var newData = _mapper.Map<ActivationCodeRequest>(dto);
                var oldData = await _uow.activationCodeRequestRepository.GetByIdAsync(newData.Id);
                if (oldData == null)
                {
                    return new NotFoundObjectResult(new {message = GetText("Contract_popup_txt_data_not_found") });
                }

                oldData.Remarks = newData.Remarks;

                await _uow.activationCodeRequestRepository.UpdateAsync(oldData);

                var json = JsonConvert.SerializeObject(new {status = 0, message = "success"});

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(new {message = e.Message});
            }
        }

        [Route("contract/uploadactivationcode/{contractUid}")]
        [HttpGet]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.ActivationCodeUpload, Permission = Permissions.Create)]
        public async Task<IActionResult> UploadACRequest(long contractUid)
        {
            try
            {
                var acReq = await _uow.GetContractWithActivationCodeRequest(contractUid);
                if (acReq == null)
                {
                    return new BadRequestObjectResult(new {message = GetText("Contract_popup_txt_request_not_found") });
                }

                if (acReq.Contract == null)
                {
                    return new BadRequestObjectResult(new {message = GetText("Contract_popup_txt_contract_not_found") });
                }

                ViewData["contract"] = acReq.Contract;
                ViewData["acReq"] = acReq;

                return View("UploadACReq");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(new {message = e.Message});
            }
        }

        [Route("contract/uploadacrequest/{requestId}")]
        [HttpPost]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.ActivationCodeUpload, Permission = Permissions.Create)]
        public async Task<IActionResult> UploadACRequest(int requestId, IFormFile file,
            [FromServices] IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                var actCodeReq = await _uow.activationCodeRequestRepository.GetByIdAsync(requestId);
                if (actCodeReq == null)
                {
                    return new NotFoundObjectResult(new {message = GetText("Contract_popup_txt_request_not_found") });
                }


                if (file == null || file.Length == 0)
                    return new NotFoundObjectResult(new {message = GetText("Contract_popup_txt_file_not_found")});
                var ext = Path.GetExtension(file.FileName);
                if (ext.ToLower() != ".csv")
                    return new BadRequestObjectResult(new {message = GetText("Contract_popup_txt_file_not_support") });

                var directory = _config["Resources:ActivationCodePath"];
                var filename = $"{actCodeReq.Id}_{file.FileName}";

                string filePath = Path.Combine(directory, filename);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var actCodeUp =
                    await _uow.activationCodeUploadRepository.GetSingleAsync(
                        a => a.ActivationCodeRequestId == actCodeReq.Id);

                if (actCodeUp != null)
                {
                    actCodeUp.UploadedFilePath = filename;
                    await _uow.activationCodeUploadRepository.UpdateAsync(actCodeUp);
                }
                else
                {
                    actCodeUp = new ActivationCodeUpload()
                    {
                        ActivationCodeRequestId = actCodeReq.Id,
                        CreateDate = DateTime.Now,
                        CreatedBy = int.Parse(ControllerContext.HttpContext.User.Claims.Where(a => a.Type == "ID")
                            .SingleOrDefault().Value),
                        UploadedFilePath = filename
                    };

                    actCodeUp = await _uow.activationCodeUploadRepository.AddAsync(actCodeUp);
                }

                _ = Task.Run(async () =>
                {
                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        await ProcessFile(uow, actCodeUp.Id, actCodeUp.CreatedBy);
                    }
                });

                var json = JsonConvert.SerializeObject(new {status = 0, message = GetText("Contract_popup_txt_processing_your_file") });

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(new {message = e.Message});
            }
        }

        private async Task ProcessFile(IUnitOfWork uow, int actCodeUpId, int userId)
        {
            try
            {
                var directory = _config["Resources:ActivationCodePath"];
                var actCodeUpload = await uow.activationCodeUploadRepository.GetByIdAsync(actCodeUpId);
                if (actCodeUpload == null)
                {
                    _logger.LogWarning($"activation code upload id {actCodeUpId} not found");
                    return;
                }

                var actCodeReq =
                    await uow.activationCodeRequestRepository.GetByIdAsync(actCodeUpload.ActivationCodeRequestId);
                if (actCodeReq == null)
                {
                    _logger.LogWarning($"activation code request id {actCodeUpload.ActivationCodeRequestId} not found");
                    return;
                }

                var contract = await uow.ContractRepository.GetByIdAsync(actCodeReq.ContractId);
                if (contract == null)
                {
                    _logger.LogWarning($"activation code contract id {actCodeReq.ContractId} not found");
                    return;
                }

                string filePath = Path.Combine(directory, actCodeUpload.UploadedFilePath);
                using (TextFieldParser textFieldParser = new TextFieldParser(filePath))
                {
                    textFieldParser.TextFieldType = FieldType.Delimited;
                    textFieldParser.SetDelimiters(new string[] {",",";"});

                    var codes = new List<ActivationCode>();
                    var today = DateTime.Now;

                    var firstLine = true;
                    while (!textFieldParser.EndOfData)
                    {
                        string[] rows = textFieldParser.ReadFields();
                        if (firstLine)
                        {
                            firstLine = false;
                            continue;
                        }
                        if (rows.Length < 2) continue;

                        codes.Add(new ActivationCode()
                        {
                            Code = rows[0],
                            ExpiryDate = DateTime.Parse(rows[1]),
                            ContractId = contract.Id,
                            CreateDate = today,
                            CreatedBy = userId,
                            ProjectId = contract.ProjectId,
                            ActivationCodeUploadId = actCodeUpId,
                            ActivationCodeRequestId = actCodeReq.Id,
                        });
                    }

                    var codeStrings = codes.Select(a => a.Code).ToList();
                    var existCodes = new List<ActivationCode>();
                    if (codeStrings != null && codeStrings.Count > 0)
                    {
                        existCodes =
                        await uow.activationCodeRepository.GetAsync(predicate: a => codeStrings.Contains(a.Code));
                    }
                    

                    if (existCodes.Count > 0)
                    {
                        _logger.LogWarning(
                            $"there is duplicate codes, {existCodes.Count} rows from total {codes.Count} rows");
                        var existCodeStrings = existCodes.Select(a => a.Code).ToList();
                        codes = codes.Where(a => !existCodeStrings.Contains(a.Code)).ToList();

                        actCodeReq.StatusUpload = 4; //duplicate code
                        await uow.activationCodeRequestRepository.UpdateAsync(actCodeReq);
                        return;
                    }

                    if (codes.Count > 0)
                    {
                        await uow.activationCodeRepository.AddRangeAsync(codes);
                    }

                    _logger.LogInformation(
                        $"processing activation code id:{actCodeUpId} done with {codes.Count} rows data");

                    var existingCodes =
                        await uow.activationCodeRepository.CountAsync(a => a.ActivationCodeRequestId == actCodeReq.Id);
                    actCodeReq.AmountCodes = existingCodes;

                    if (existingCodes == contract.ActivationCodes)
                    {
                        actCodeReq.StatusUpload = 1;
                        //actCodeReq.Remarks = GetText("Upload completed);
                    }
                    else if (existingCodes > contract.ActivationCodes)
                    {
                        actCodeReq.StatusUpload = 3;
                        //actCodeReq.Remarks = GetText("Code too many);
                    }
                    else
                    {
                        actCodeReq.StatusUpload = 2;
                        //actCodeReq.Remarks = GetText("Code not enough);
                    }

                    await uow.activationCodeRequestRepository.UpdateAsync(actCodeReq);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"error while processing file id:{actCodeUpId} | {e.Message}", e);
                return;
            }
        }
    }
}