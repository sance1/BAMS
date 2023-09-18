using BAMS.Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EightElements.Services.Default
{
    public class LanguageProvider : ILanguageProvider
    {        
        private IHttpContextAccessor _contextAccessor;
        private IUnitOfWork _uow;

        public LanguageProvider(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _contextAccessor = httpContextAccessor;
            _uow = unitOfWork;
        }

        public string GetLanguage()
        {
            string language = GetLanguageFromCookie();            
            if (!string.IsNullOrEmpty(language)) return language;

            language = GetPreferredLanguage();
            if (!string.IsNullOrEmpty(language)) return language;

            language = GetClientLanguage();
            if (!string.IsNullOrEmpty(language)) return language;

            return null;
        }


        private string GetLanguageFromCookie()
        {
            _contextAccessor.HttpContext.Request.Cookies.TryGetValue(
                CookieRequestCultureProvider.DefaultCookieName,
                out string language);

            return language;
        }

        private string GetPreferredLanguage()
        {
            try
            {
                var httpContext = _contextAccessor.HttpContext;
                if (httpContext == null) return null;

                var userClaim = httpContext.User.Claims.SingleOrDefault(a => a.Type == "ID");
                if (userClaim == null) return null;

                int.TryParse(userClaim.Value, out int userId);
                string preferredLanguage = _uow.AccountRepository.GetPreferredLanguage(userId);
                return preferredLanguage;

            } catch (Exception e)
            {
                return null;
            }
        }

        private string GetClientLanguage()
        {
            var languages = _contextAccessor.HttpContext.Request.Headers["Accept-Language"];
            if (languages.Count == 0 || languages[0] == "*") return "en";

            var match = Regex.Match(languages[0], "(?<language>\\w+)-\\w+");
            return (match.Length == 0)
                ? "en"
                : match.Groups["language"].Value;
        }


    }
}
