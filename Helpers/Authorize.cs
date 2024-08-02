using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopBackgroundSystem.Models;
using System.Data;

namespace ShopBackgroundSystem.Helpers
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "登录了吗您呐" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
    public class AuthRoleAttribute : Attribute, IAuthorizationFilter
    {
        
        string[] Requiredrole;
        public AuthRoleAttribute(string[] requiredrole)
        {
            Requiredrole = requiredrole;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var thisRole = (string)context.HttpContext.Items["Role"];
            if (!Requiredrole.Contains(thisRole))
            {
                context.Result = new JsonResult(new { message = "身份验证失败" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}
