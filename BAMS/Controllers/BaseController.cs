using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Helpers;
using EightElements.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace BAMS.Controllers
{
    public class BaseController : Controller
    {
        private IUnitOfWork _uow;
        private ITextService _textService;
        protected string _projectIdTest;
        private Account _account;


        public BaseController(
            IUnitOfWork unitOfWork,
            ITextService textService)
        {
            this._uow = unitOfWork;
            _textService = textService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (UserId == 0 && _account != null)
            {
                _account = null;
            }

            if (_account == null && UserId > 0)
            {
                _account = await _uow.AccountRepository.GetByIdAsync(UserId);
            }

            ViewBag.ClientLanguage = GetClientLanguage();

            await base.OnActionExecutionAsync(context, next);
        }


        public string GetClientLanguage()
        {
            var header = Request.Headers["Accept-Language"];
            if (header.Count == 0 || header[0] == "*") return "en";

            var match = Regex.Match(header[0], "(?<language>\\w+)-\\w+");
            return (match.Length == 0)
                ? "en"
                : match.Groups["language"].Value;
        }


        protected void SetUserPreference(string preference, string value)
        {
        }


        protected ContentResult Success()
        {
            return Content(
                JsonResponseBuilder.GetSuccessResponse(),
                "application/json");
        }

        protected ContentResult Success(string name, JToken content)
        {
            string result = JsonResponseBuilder
                .GetSuccessResponseBuilder()
                .SetProperty(name, content)
                .Build();

            return Content(result, "application/json");
        }

        protected ContentResult Failed(string message)
        {
            return Content(
                JsonResponseBuilder.GetFailedResponse(message),
                "application/json");
        }


        protected ContentResult Error(string message)
        {
            return Content(
                JsonResponseBuilder.GetErrorResponse(message),
                "application/json");
        }

        protected string GetText(string key)
        {
            string languageCode = _account?.PreferredLanguage;

            if (languageCode == null)
            {
                languageCode = GetClientLanguage();
            }

            if (languageCode == null)
            {
                languageCode = "en";
            }

            return _textService.GetString(key, languageCode);
        }

        public string UserName
        {
            get { return GetClaimItem(ClaimTypes.Name); }
        }

        public int UserId
        {
            get
            {
                int.TryParse(GetClaimItem("ID"), out int userId);
                return userId;
            }
        }


        protected int ProjectId
        {
            get
            {
                int.TryParse(GetClaimItem("ProjectId"), out int projectId);
                return projectId;
            }
        }

        protected int ContractId
        {
            get
            {
                int.TryParse(GetClaimItem("ContractId"), out int contractId);
                return contractId;
            }
        }

        protected int DistrictId
        {
            get
            {
                int.TryParse(GetClaimItem("DistrictId"), out int districtId);
                return districtId;
            }
        }

        protected int AdministrativeUnitId
        {
            get
            {
                int.TryParse(GetClaimItem("AdministrativeUnitId"), out int administrativeUnitId);
                return administrativeUnitId;
            }
        }

        protected int SchoolId
        {
            get
            {
                int.TryParse(GetClaimItem("SchoolId"), out int schoolId);
                return schoolId;
            }
        }

        protected int RoleId
        {
            get
            {
                int.TryParse(GetClaimItem(ClaimTypes.Role), out int roleId);
                return roleId;
            }
        }


        private string GetClaimItem(string key)
        {
            try
            {
                var claim = HttpContext?.User.Claims
                    .SingleOrDefault(a => a.Type == key);

                return claim?.Value;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}