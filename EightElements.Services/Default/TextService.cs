using BAMS.Data.Interface;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EightElements.Services.Default
{
    public class TextService : ITextService
    {
        private IUnitOfWork _uow;
        private IConfiguration _config;
        private IHttpContextAccessor _http;
        private string _preferredLanguage;

        public TextService(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            ILanguageProvider languageProvider)
        {
            _config = configuration;
            _http = httpContextAccessor;
            _uow = unitOfWork;

            _preferredLanguage = languageProvider.GetLanguage();
        }


        public IHtmlContent GetHtml(string key, string language = null)
        {
            if (string.IsNullOrEmpty(language)) language = _preferredLanguage;
            //_http.HttpContext.Request.Cookies.TryGetValue(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    out language);
            string defaultLanguage = _config["Settings:DefaultLanguage"];

            return new HtmlString(_uow.pageTextRepository.GetPortalText(key, language, defaultLanguage));

        }


        public string GetString(string key, string language = null)
        {
            if (string.IsNullOrEmpty(language)) language = _preferredLanguage;
            //_http.HttpContext.Request.Cookies.TryGetValue(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    out language);
            string defaultLanguage = _config["Settings:DefaultLanguage"];

            return _uow.pageTextRepository.GetPortalText(key, language, defaultLanguage);

        }

    }
}
