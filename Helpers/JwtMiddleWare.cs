using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopBackgroundSystem.Helpers;
using ShopBackgroundSystem.Models;
using ShopBackgroundSystem.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ShopServerSystem.Helpers
{
    public class JwtMiddleWare
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly AuthSettings _authSettings;
        public JwtMiddleWare(RequestDelegate requestDelegate, IOptions<AuthSettings> authSettings)
        {
            _requestDelegate = requestDelegate;
            _authSettings = authSettings.Value;
        }

        //验证从发送方标头提取的令牌
        public async Task Invoke(HttpContext context, IUserService service)
        {
            //从上下文中拿到token
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                //验证
                try
                {
                    AttachUserToContext(context, service, token);
                }
                catch (SecurityTokenExpiredException)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsJsonAsync(new {message = "token已经过期，请重新登录。" });
                    return;
                }
            }
            //调用下一个中间件
            await _requestDelegate(context);
        }
        //如果验证通过 获取数据
        private void AttachUserToContext(HttpContext context, IUserService service, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_authSettings.Secret);
            tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out var validatedToken
            );
            //转换成JwtSecurityToken
            var jwtToken = (JwtSecurityToken)validatedToken;
            //获取用户账号
            var uaccount = jwtToken.Claims.First(c => c.Type == "uaccount").Value;
            var userRole = jwtToken.Claims.First(c => c.Type == "role").Value;
            Console.WriteLine(jwtToken);
            context.Items["User"] = new User { Uaccount = uaccount, Upwd = "default", Utype = userRole };
            context.Items["Role"] = userRole;
        }
    }
}
