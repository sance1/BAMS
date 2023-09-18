using System;
using System.Collections.Generic;
using System.Linq;
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
    public class RoleController : BaseController
    {
        private IUnitOfWork unitOfWork;
        private IMapper _mapper { get; set; }
        private ITextService _textService;

        public RoleController(IUnitOfWork unitOfWork, ITextService textService) 
            : base(unitOfWork, textService)
        {
            this.unitOfWork = unitOfWork;
            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<RoleDTO, Role>();
                    cfg.CreateMap<Role, RoleDTO>();
                    cfg.CreateMap<Role, Role>();

                    cfg.CreateMap<AccessPermissionDTO, AccessPermission>();
                    cfg.CreateMap<AccessPermission, AccessPermissionDTO>();
                    cfg.CreateMap<AccessPermission, AccessPermission>();
                });

                _mapper = config.CreateMapper();
            }
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Role, Permission = Permissions.Read)]
        public IActionResult Index()
        {
            return View();
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Role, Permission = Permissions.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.Id = 0;
            ViewBag.Role = new Role();
            return View("CreateOrUpdate");
        }

        [PermitAccess(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Role, Permission = Permissions.Update)]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Id = id;
            ViewBag.Role = await unitOfWork.RoleRepository.GetSingleAsync(ar => ar.Id == id);
            return View("CreateOrUpdate");
        }

        //API
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await unitOfWork.RoleRepository.GetSingleAsync(ac => ac.Id == id);
            var account = await unitOfWork.AccountRepository.CountAsync(a => a.RoleId == role.Id);
            if (account > 1)
            {
                return Content(
                    JsonResponseBuilder.GetFailedResponse(
                        GetText("Role_popup_txt_cannot_delete_role")),
                    "application/json");
            }

            role.DeleteDate = DateTime.Now;
            role.DeletedBy = 1;
            await unitOfWork.SaveAsync();

            var rp = await unitOfWork.rolePermissionRepository.ToListAsync(rp => rp.RoleId == id);
            foreach (var val in rp)
            {
                val.DeleteDate = DateTime.Now;
                val.DeletedBy = 1;
                await unitOfWork.SaveAsync();
            }

            return Content("ok");
        }

        public async Task<IActionResult> CreateOrUpdateRole(string name, int accessLevel, string rolePermission, int id)
        {
            var role = await unitOfWork.RoleRepository.GetSingleAsync(ac => ac.Id == id);
            if (role == null)
            {
                role = new Role();
                role.Name = name;
                role.AccessLevel = (byte) accessLevel;
                await unitOfWork.RoleRepository.AddAsync(role);
            }
            else
            {
                role.Name = name;
                role.AccessLevel = (byte) accessLevel;
                await unitOfWork.SaveAsync();
            }

            var rolePermissionAccessDto = JsonConvert.DeserializeObject<List<RolePermissionAccessDTO>>(rolePermission);
            foreach (var val in rolePermissionAccessDto)
            {
                var rolep = await unitOfWork.rolePermissionRepository.GetSingleAsync(ac =>
                    ac.RoleId == role.Id && ac.Group == val.key);
                if (rolep != null)
                {
                    rolep.Access = val.value;
                    await unitOfWork.SaveAsync();
                }
                else
                {
                    rolep = new RolePermission();
                    rolep.RoleId = role.Id;
                    rolep.Group = val.key;
                    rolep.Access = val.value;
                    rolep.CreateDate = DateTime.Now;
                    rolep.CreatedBy = 1;
                    await unitOfWork.rolePermissionRepository.AddAsync(rolep);
                }
            }

            return Content("ok");
        }

        [HttpPost]
        public async Task<IActionResult> GetListRole(DtParameters dtParameters)
        {
            try
            {
                var searchValue = dtParameters.Search.Value;
                int pageSize = dtParameters.Length;
                int skip = dtParameters.Start;
                int recordsTotal = 0;

                var pred = PredicateBuilder.True<Role>();
                foreach (var col in dtParameters.Columns)
                {
                    if (!string.IsNullOrEmpty(col.Search.Value))
                    {
                        switch (col.Name)
                        {
                            case "Name":
                                pred = pred.And(a => a.Name.Contains(col.Search.Value));
                                break;
                            case "AccessLevel":
                                pred = pred.And(a => a.AccessLevel.ToString().Contains(col.Search.Value));
                                break;
                        }
                    }
                }

                if (dtParameters.Columns.Length > 0)
                {
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        pred = pred.And(a => a.Name.Contains(searchValue));
                    }
                }

                var data = await unitOfWork.RoleRepository.GetAsync(o => o.OrderBy(p => p.Id), pred, true, pageSize,
                    skip);
                recordsTotal = await unitOfWork.RoleRepository.CountAsync(pred);

                var result = new DtResult<RoleDTO>()
                {
                    Data = _mapper.Map<List<RoleDTO>>(data),
                    Draw = dtParameters.Draw,
                    RecordsFiltered = data.Count,
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

        public async Task<IActionResult> GetListRolePermission(int id)
        {
            var pred = PredicateBuilder.True<RolePermission>();
            pred = pred.And(a => a.RoleId == id);
            var data = await unitOfWork.rolePermissionRepository.GetAsync(o => o.OrderBy(p => p.Id), pred, false,
                0,
                0);
            return Content(JsonConvert.SerializeObject(data), "application/json");
        }

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
                        }
                    }
                }

                if (dtParameters.Columns.Length > 0)
                {
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        pred = pred.And(a => a.Name.Contains(searchValue) || a.Group.Contains(searchValue));
                    }
                }

                var data = await unitOfWork.accessRepository.GetAsync(o => o.OrderBy(p => p.Group), pred, true,
                    pageSize,
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
    }
}