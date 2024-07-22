using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopBackgroundSystem.Helpers;
using ShopBackgroundSystem.Models;
using ShopBackgroundSystem.Services.Interfaces;
using ShopBackgroundSystem.VM;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopServerSystem.Services.Implementation
{
    public class UserService : IUserService
    {
        private User _user;
        private readonly AuthSettings _authSettings;

        public UserService(IOptions<AuthSettings> authSettings) 
        { 
            _authSettings = authSettings.Value;
        }
        public UserResponseVM Authenticate(User? user)
        {
            //通过账号密码验证
            if(user!=null)
            {
                _user = user;
                //创建令牌
                var token = GenerateJwtToken(_user);
                //返回包含用户信息的令牌
                return new UserResponseVM(_user, token);
            }
            return null;
        }
        //创建令牌
        private string GenerateJwtToken(User user)
        {
            byte[] key = Encoding.ASCII.GetBytes(_authSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //获取或者设置身份信息
                Subject=new ClaimsIdentity (
                    new[]
                    {
                        new Claim("uaccount",user.Uaccount),
                        new Claim("role",user.Utype)
                    }),
                //过期时间
                Expires = DateTime.UtcNow.AddMinutes(20),
                //证书签名
                SigningCredentials=new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                ),
            };
            //创建token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public User GetByAccount(string uaccount)
        {
            if (uaccount == _user.Uaccount)
                return _user;
            else return null;
        }
    }
}
