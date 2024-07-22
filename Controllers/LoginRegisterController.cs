using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBackgroundSystem.Helper;
using ShopBackgroundSystem.Models;
using ShopBackgroundSystem.Services.Interfaces;
using ShopBackgroundSystem.VM;

namespace ShopBackgroundSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRegisterController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly CustomerDbContext _dbContext;

        public LoginRegisterController(IUserService userService, CustomerDbContext dbContext)
        {
            _userService = userService;
            _dbContext = dbContext;
        }
        [HttpPost("auth")]
        public async Task<ActionResult<UserResponseVM>> Login(UserVM model)
        {
            model.Upwd = MD5Helper.GetMD5(model.Uaccount + model.Upwd);
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Uaccount == model.Uaccount && u.Upwd == model.Upwd);
            var response = _userService.Authenticate(user);
            if (response == null)
            {
                return BadRequest(new { message = "用户名或密码不正确!" });
            }
            return Ok(response);
        }
    }
}
