using System;
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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BAMS.Controllers
{
    public class MenuController : BaseController
    {
        private IUnitOfWork unitOfWork;
        private IMapper _mapper { get; set; }
        private ITextService _textService;
        public MenuController(IUnitOfWork unitOfWork, ITextService textService) 
            : base(unitOfWork, textService)
        {
            _textService = textService;
            this.unitOfWork = unitOfWork;
            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<AccessPermissionDTO, AccessPermission>();
                    cfg.CreateMap<AccessPermission, AccessPermissionDTO>();
                    cfg.CreateMap<AccessPermission, AccessPermission>();
                });

                _mapper = config.CreateMapper();
            }
        }

        // GET
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Menu, Permission = Permissions.Read)]
        public async Task<IActionResult> Index(int id = 0)
        {
            ViewData["Title"] = GetText(_textService.GetString("Access_permissions_title_access_permissions","en"));
            //ViewData["MenuActived"] = GetText("Access Permission");
            ViewBag.Id = id;
            ViewBag.Menu = new AccessPermission();
            if (id > 0)
            {
                ViewBag.Menu = await unitOfWork.accessRepository.GetSingleAsync(ar => ar.Id == id);
            }
            return View();
        }
        
        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Menu, Permission = Permissions.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.Id = 0;
            ViewBag.Menu = new AccessPermission();
            return View("CreateOrUpdate");
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Menu, Permission = Permissions.Update)]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Id = id;
            ViewBag.Menu = await unitOfWork.accessRepository.GetSingleAsync(ar => ar.Id == id);
            return View("CreateOrUpdate");
        }


        //API
        [HttpPost]
        public async Task<IActionResult> GetListMenu(DtParameters dtParameters)
        {
            try
            {
                var searchValue = dtParameters.Search.Value;
                int pageSize = dtParameters.Length;
                int skip = dtParameters.Start;
                int recordsFiltered = 0;
                int recordsTotal = 0;

                var pred = PredicateBuilder.True<AccessPermission>();
                foreach (var col in dtParameters.Columns)
                {
                    if (!string.IsNullOrEmpty(col.Search.Value))
                    {
                        switch (col.Name)
                        {
                            case "Name":
                                pred = pred.And(a => a.Name.Contains(col.Search.Value));
                                break;
                            case "Group":
                                pred = pred.And(a => a.Group.Contains(col.Search.Value));
                                break;
                            case "Permission":
                                pred = pred.And(a => a.Permission.ToString().Contains(col.Search.Value));
                                break;
                        }
                    }
                }

                if (dtParameters.Columns.Length > 0)
                {
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        pred = pred.And(a => a.Name.Contains(searchValue) || a.Group.Contains(searchValue)
                        );
                    }
                }

                var data = await unitOfWork.accessRepository.GetAsync(o => o.OrderBy(p => p.Id), pred, true, pageSize,
                    skip);
                recordsFiltered = await unitOfWork.accessRepository.CountAsync(pred);
                recordsTotal = await unitOfWork.accessRepository.CountAsync(a => true);

                var result = new DtResult<AccessPermissionDTO>()
                {
                    Data = _mapper.Map<List<AccessPermissionDTO>>(data),
                    Draw = dtParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal
                };

                var json = JsonConvert.SerializeObject(result);

                return Content(json, "application/json");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
        
        public async Task<IActionResult> CreateOrUpdateMenu(string name, string group, int permission,string menuUrl,int menuOrder, int id)
        {
            var role = await unitOfWork.accessRepository.GetSingleAsync(ac => ac.Id == id);
            var getMenuOrder = await unitOfWork.accessRepository.GetSingleAsync(ac => ac.MenuOrder == menuOrder && ac.Id != id);
            if (menuOrder == 0 && permission == 8) {
                return Content(JsonConvert.SerializeObject(new { message = GetText("Access_permissions_popup_menu_order_cant_insert") }), "application/json");
            }
            if (getMenuOrder != null && permission == 8 ) {
                return Content(JsonConvert.SerializeObject(new { message = GetText("Access_permissions_popup_menu_order_duplicate") }), "application/json");
            }
            if (role == null)
            {
                role = new AccessPermission();
                role.Name = name;
                role.Group = group;
                role.Permission = permission;
                role.MenuUrl = menuUrl;
                role.MenuOrder = menuOrder;
                await unitOfWork.accessRepository.AddAsync(role);
            }
            else
            {
                role.Name = name;
                role.Group = group;
                role.Permission = permission;
                role.MenuOrder = menuOrder;
                await unitOfWork.SaveAsync();
            }
            return Content("ok");
        }
        
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var role = await unitOfWork.accessRepository.GetSingleAsync(ac => ac.Id == id);
            role.DeleteDate = DateTime.Now;
            role.DeletedBy = 1;
            await unitOfWork.SaveAsync();
            return Content("ok");
        }

        public async Task<IActionResult> GetMenuAccessPermission([FromServices] ITextService textService)
        {
            var getAccessPermission = await unitOfWork.accessRepository.GetMenuAccessPermission();

            var role = ControllerContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            int.TryParse(role, out int roleId);

            var rolePermission = (await unitOfWork.rolePermissionRepository
                .GetAsync(predicate: rp => rp.RoleId == RoleId && (8 & rp.Access) == 8))
                .Select(a => a.Group + "_sidebar").ToList();

            if(rolePermission.Count == 0)
            {
                return Content(JsonConvert.SerializeObject(new {message = GetText("Access_permissions_popup_role_not_found") }), "application/json");
            }

            getAccessPermission = getAccessPermission.Where(a => rolePermission.Contains(a.Name)).ToList();

            foreach(var menu in getAccessPermission)
            {
                menu.Name = GetText(menu.Name);
            }

            var getData = new { getAccessPermission };
            return Content(JsonConvert.SerializeObject(getData), "application/json");
        }
    }
}