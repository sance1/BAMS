using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BAMS.Controllers
{
    public class TrackingController : Controller
    {
        private IUnitOfWork _uow;
        private readonly ILogger<TrackingController> _logger;

        private int _projectId = 0;
        private int _districtId = 0;

        public TrackingController(IUnitOfWork unitOfWork, ILogger<TrackingController> logger,
            IHttpContextAccessor contextAccessor)
        {
            _uow = unitOfWork;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string encryptedParams = Request.QueryString.ToString();
            int eventId = 0;

            int.TryParse(Request.Query["eventId"], out eventId);

            var log = new LogTracking()
            {
                Data = encryptedParams,
                CreateDate = DateTime.Now,
                EventId = eventId
            };
            await _uow.logTrackingRepository.AddAsync(log);

            if (eventId > 1)
            {
                var username = Request.Query["username"];
                if (string.IsNullOrEmpty(username))
                {
                    username = Request.Query["msisdn"];
                }

                var user = await _uow.UserAccountRepository.GetSingleAsync(ua => ua.UserName == username);
                if (user == null)
                {
                    user = new UserAccount();
                    user.UserName = username;
                    await _uow.UserAccountRepository.AddAsync(user);
                    await _uow.SaveAsync();
                }
            }

            return Content("ok");
        }
    }
}