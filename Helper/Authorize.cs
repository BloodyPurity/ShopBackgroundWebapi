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
                context.Result = new JsonResult(new { message = "未授权" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
    public class AuthRoleAttribute : Attribute, IAuthorizationFilter
    {
        string[] Role;
        public AuthRoleAttribute(string[] role)
        {
            Role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var thisRole = (string)context.HttpContext.Items["Role"];
            if (!Role.Contains(thisRole))
            {
                context.Result = new JsonResult(new { message = "非法身份" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}
