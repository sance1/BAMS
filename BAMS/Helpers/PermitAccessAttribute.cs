using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using BAMS.Controllers;
using BAMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BAMS.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermitAccessAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Group { get; set; }
        public int Permission { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var dbContext = context.HttpContext.RequestServices.GetRequiredService<DataContext>();
                var role = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                int.TryParse(role, out int roleId);
                var rolePermission = dbContext.RolePermission.SingleOrDefault(rp => rp.RoleId == roleId && rp.Group == Group);
                if (rolePermission == null)
                {
                    context.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new{ controller = "Error", action = "AccessDenied" }));
                    return;
                }
                var data = dbContext.AccessPermission.SingleOrDefault(ap =>
                    ap.Group == Group && ap.Permission == Permission && (ap.Permission & rolePermission.Access) == Permission);
                //
                if (data == null)
                {
                    context.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new{ controller = "Error", action = "AccessDenied" }));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}