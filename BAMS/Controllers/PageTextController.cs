using AutoMapper;
using BAMS.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BAMS.Data.Models;
using BAMS.Models;
using BAMS.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Linq.Expressions;
using BAMS.InputValidators;
using Z.EntityFramework.Plus;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using EightElements.Services;

namespace BAMS.Controllers
{
    public class PageTextController : BaseController
    {
        private IUnitOfWork _uow;
        private IMapper _mapper { get; set; }
        private readonly ILogger<PageTextController> _logger;
        private ITextService _textService;
        public PageTextController(IUnitOfWork unitOfWork, ILogger<PageTextController> logger, ITextService textService)
            : base(unitOfWork, textService)
        {
            _uow = unitOfWork;
            _logger = logger;
            _textService = textService;

            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CreatePageTextDto, PageText>();
                    cfg.CreateMap<PageText, CreatePageTextDto>();
                    cfg.CreateMap<UpdatePageTextDto, PageText>(MemberList.Source);
                    cfg.CreateMap<PageText, UpdatePageTextDto>();
                    cfg.CreateMap<ReadPageTextDto, PageText>();
                    cfg.CreateMap<PageText, ReadPageTextDto>();
                    cfg.CreateMap<PageText, PageText>();
                });

                _mapper = config.CreateMapper();
            }
        }

        [Route("PageText")]
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.PageText, Permission = Permissions.Read)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.PageText,
            Permission = Permissions.Read)]

        public async Task<IActionResult> GetListPageText(DtParameters dtParameters)
        {
            try
            {

                var searchValue = dtParameters.Search.Value;
                int pageSize = dtParameters.Length;
                int skip = dtParameters.Start;
                int recordsFiltered = 0;
                int recordsTotal = 0;

                var pred = PredicateBuilder.True<PageText>();
                foreach (var col in dtParameters.Columns)
                {
                    if (!string.IsNullOrEmpty(col.Search.Value))
                    {
                        switch (col.Name)
                        {
                            case "Key":
                                pred = pred.And(a => a.Key.Contains(col.Search.Value));
                                break;
                            case "Text":
                                pred = pred.And(a => a.Text.Contains(col.Search.Value));
                                break;
                            case "LanguageCode":
                                pred = pred.And(a => a.LanguageCode.Contains(col.Search.Value));
                                break;
                        }
                    }
                }

                var colSort = dtParameters.Columns[dtParameters.Order[0].Column];
                var dir = dtParameters.Order[0].Dir;
                Expression<Func<PageText, dynamic>> sortFunc = a => a.Id;

                switch (colSort.Name)
                {

                    case "Key":
                        sortFunc = a => a.Key;
                        break;
                    case "Text":
                        sortFunc = a => a.Text;
                        break;
                    case "LanguageCode":
                        sortFunc = a => a.LanguageCode;
                        break;
                   /* default:
                        sortFunc = a => a.CreateDate;
                        break;*/
                }

                Func<IQueryable<PageText>, IOrderedQueryable<PageText>> orderBy = o => o.OrderBy(sortFunc);
                if (dir == DtOrderDir.Desc)
                {
                    orderBy = o => o.OrderByDescending(sortFunc);
                }

                var data = await _uow.pageTextRepository.GetAsync(orderBy, pred, true, pageSize, skip);
                recordsFiltered = await _uow.pageTextRepository.CountAsync(pred);
                recordsTotal = await _uow.pageTextRepository.CountAsync(a => true);

                var result = new DtResult<ReadPageTextDto>()
                {
                    Data = _mapper.Map<List<ReadPageTextDto>>(data),
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

        [Route("new-PageText")]
        [Route("PageText/{uid}")]
        [Route("PageText/detail")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.PageText,
            Permission = Permissions.Read)]
        public async Task<IActionResult> Detail(long uid = 0)
        {
            if (uid > 0)
            {
                try
                {
                    ViewData["PageText"] = await _uow.pageTextRepository.GetByUidAsync(uid);
                }
                catch (Exception e)
                {
                    _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                    return new BadRequestObjectResult(new { message = e.Message });
                }

            }

            return View("CreateUpdate");
        }

        [Route("PageText/create")]
        [Route("create-PageText")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.PageText,
            Permission = Permissions.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePageTextDto dto)
        {
            try
            {
                var validationResults =
                    await PageTextValidator.ValidateCreatePageText(dto, _uow, _textService);

                if (validationResults.Count > 0)
                {
                    return new BadRequestObjectResult(new { message = string.Join("<br/>", validationResults) });
                }

                var pageText = _mapper.Map<PageText>(dto);
                pageText.CreateDate = DateTime.Now;
                pageText.CreatedBy = int.Parse(ControllerContext.HttpContext.User.Claims.FirstOrDefault(a => a.Type == "ID").Value);             
                pageText.Uid = await _uow.pageTextRepository.GenerateUid();
                await _uow.pageTextRepository.AddAsync(pageText);

                var json = JsonConvert.SerializeObject(new { status = 0, message = GetText("Pagetexts_popup_success") });

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(new { message = e.Message });
            }
        }


        [Route("update-PageText")]
        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.PageText,
            Permission = Permissions.Update)]
        public async Task<IActionResult> Update([FromBody] UpdatePageTextDto dto)
        {
            try
            {
                var pageText = await _uow.pageTextRepository.GetByUidAsync(dto.Uid);
                if (pageText == null)
                {
                    return new NotFoundObjectResult(new { message = GetText("Pagetexts_popup_data_not_found") });
                }

                var validationResults =
                    await PageTextValidator.ValidateUpdatePageText(pageText.Id, dto, _uow,_textService);

                if (validationResults.Count > 0)
                {
                    return new BadRequestObjectResult(new { message = string.Join("<br/>", validationResults) });
                }

                pageText.Key = dto.Key;
                pageText.Text = dto.Text;
                pageText.LanguageCode = dto.LanguageCode;

                await _uow.pageTextRepository.UpdateAsync(pageText);

                // todo: save update history

                var json = JsonConvert.SerializeObject(new { status = 0, message = GetText("Pagetexts_popup_success") });

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(new { message = e.Message });
            }
        }

        [HttpGet]
        [Route("delete-PageText/{uid}")]
        [Route("PageText/delete/{uid}")]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.PageText,
            Permission = Permissions.Delete)]
        public async Task<IActionResult> Delete(long uid)
        {
            try
            {
                var pageText = await _uow.pageTextRepository.GetByUidAsync(uid);
                if (pageText == null)
                {
                    return new NotFoundObjectResult(new { message = GetText("Pagetexts_popup_data_not_found") });
                }
                pageText.DeleteDate = DateTime.Now;
                pageText.DeletedBy = int.Parse(ControllerContext.HttpContext.User.Claims.Where(a => a.Type == "ID").SingleOrDefault().Value);
                await _uow.pageTextRepository.UpdateAsync(pageText);

                var json = JsonConvert.SerializeObject(new { message = GetText("Pagetexts_popup_success") });

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return new BadRequestObjectResult(new { message = e.Message });
            }
        }
        [Route("ClearText")]
        public string ClearText()
        {
            QueryCacheManager.ExpireAll();
            return "ok";
        }

        [HttpGet]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName, culture,
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            CultureInfo.CurrentCulture = new CultureInfo(culture);

            return LocalRedirect(returnUrl);
        }


    }
}
