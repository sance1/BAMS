using BAMS.Data.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BAMS.Controllers
{
    public class HomeController : BaseController
    {
        private IUnitOfWork _uow;
        
        public HomeController(IUnitOfWork unitOfWork)
            : base(unitOfWork, null)
        {
            _uow = unitOfWork;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index()
        {
            var districtRoles = new int[] { 13 };
            var teacherRoles = new int[] { 14 };
            var govRoles = new int[] { 18 };

            if (govRoles.Contains(RoleId))
            {
                var project = await _uow.ProjectRepository.GetByIdAsync(ProjectId);
                var districtQty = await _uow.DistrictRepository.CountAsync(a => a.ProjectId == ProjectId);
                var schoolQty = await _uow.SchoolRepository.CountAsync(a => a.ProjectId == ProjectId);
                var teacherQty = await _uow.AccountRepository.CountAsync(a => a.ProjectId == ProjectId);
                var userAccQty = await _uow.UserAccountRepository.CountAsync(a => a.ProjectId == ProjectId);

                ViewData["districtQty"] = districtQty;
                ViewData["schoolQty"] = schoolQty;
                ViewData["teacherQty"] = teacherQty;
                ViewData["userAccQty"] = userAccQty;
                ViewData["project"] = project;

                return View("Index_Gov");
            }
            if (districtRoles.Contains(RoleId))
            {
                var district = await _uow.DistrictRepository.GetByIdAsync(DistrictId);
                var accesLevel = await _uow.RoleRepository.GetByIdAsync(RoleId);
                var lv = await _uow.RoleRepository.ToListAsync(r => r.AccessLevel > accesLevel.AccessLevel);
                var arr = lv.Select(a => a.Id).ToArray();
                var schoolQty = await _uow.SchoolRepository.CountAsync(a => a.DistrictId == DistrictId);
                var teacherQty = await _uow.AccountRepository.CountAsync(a => a.DistrictId == DistrictId && a.ProjectId != 0 && arr.Contains(a.RoleId));
                var userAccQty = await _uow.UserAccountRepository.CountAsync(a => a.DistrictId == DistrictId);

                ViewData["district"] = district;
                ViewData["schoolQty"] = schoolQty;
                ViewData["teacherQty"] = teacherQty;
                ViewData["userAccQty"] = userAccQty;

                return View("Index_District");
            }
            if (teacherRoles.Contains(RoleId))
            {
                var school = await _uow.SchoolRepository.GetByIdAsync(SchoolId);
                var activatedUser = await _uow.UserAccountRepository.CountAsync(a => a.SchoolId == SchoolId && a.ActivationStatus == 1);
                var realTimeLogin = 0;

                ViewData["school"] = school;
                ViewData["activatedUser"] = activatedUser;
                ViewData["realTimeLogin"] = realTimeLogin;

                return View("Index_School");
            }

            if (RoleId == 12) 
            {
                return View("Index_Educa");
            }

            if (RoleId == 1)
            {
                ViewData["TotalProject"] = await _uow.ProjectRepository.CountAsync();
                ViewData["TotalContract"] = await _uow.ContractRepository.CountAsync();
                ViewData["TotalDistrict"] = await _uow.DistrictRepository.CountAsync();
                ViewData["TotalSchool"] = await _uow.SchoolRepository.CountAsync();
                ViewData["TotalTeacher"] = await _uow.AccountRepository.CountAsync(predicate: a => teacherRoles.Contains(a.RoleId));
                ViewData["TotalStudents"] = await _uow.UserAccountRepository.CountAsync();
            }
            return View("Index_Admin");
        }

        public IActionResult Teacher()
        {
            return View();
        }
    }
}
