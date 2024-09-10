using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBackgroundSystem.Helpers;
using ShopBackgroundSystem.Models;
using ShopBackgroundSystem.Services.Interfaces;
using ShopBackgroundSystem.VM;

namespace ShopBackgroundSystem.Controllers
{

    [Route("1.0/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly RSAHelper _RSAHelper;
        private readonly IUserService _userService;
        private readonly CustomerDbContext _dbContext;

        public AccountController(RSAHelper rSAHelper, IUserService userService, CustomerDbContext dbContext)
        {
            _RSAHelper = rSAHelper;
            _userService = userService;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponseVM>> Login(UserVM model)
        {
            model.Upwd = _RSAHelper.Decrypt(model.Upwd);
            model.Upwd = MD5Helper.GetMD5(model.Uaccount + model.Upwd);
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Uaccount == model.Uaccount && u.Upwd == model.Upwd);
            var response = _userService.Authenticate(user);
            if (response == null)
            {
                return BadRequest(new { message = "账号或密码错误" });
            }
            return Ok(response);
        }

        [HttpPost("myinfo")]
        [Authorize,AuthRole(requiredrole: new string[] {"admin","user"})]
        public async Task<ActionResult<User>> Myinfo(string uaccount)
        {
            var user = await _dbContext.Users.Where(u => u.Uaccount == uaccount)
                .Select(p => new { p.Uname, p.Ubirth, p.Utype, p.Uphone })
                .SingleOrDefaultAsync();
            return Ok(user);
        }

        [HttpPost("adminregist")]
        public async Task<ActionResult> AdminRegist(UserVM model)
        {
            model.Upwd = MD5Helper.GetMD5(model.Uaccount + _RSAHelper.Decrypt(model.Upwd));
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Utype == "admin");
            if (user != null)
            {
                return BadRequest(new { message = "管理员已存在" });
            }
            User newUser = new User { Uaccount = model.Uaccount,Upwd = model.Upwd ,Utype = "admin" };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "注册成功" });
        }

        [HttpPut("password")]
        public async Task<ActionResult> Password(UserPwdChangeVM model)
        {
            model.Upwd = _RSAHelper.Decrypt(model.Upwd);
            model.newUpwd = _RSAHelper.Decrypt(model.newUpwd);
            model.Upwd = MD5Helper.GetMD5(model.Uaccount + model.Upwd);
            model.newUpwd = MD5Helper.GetMD5(model.Uaccount + model.newUpwd);
            if (model.Upwd == model.newUpwd)
            {
                return BadRequest(new { message = "修改密码一致" });
            }
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Uaccount == model.Uaccount && u.Upwd == model.Upwd);
            if (user == null)
            {
                return BadRequest(new { message = "账号或密码出错" });
            }
            else user.Upwd = model.newUpwd;
            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "重新登录" });
        }
        /// <summary>
        /// 抄来的函数原型
        /// </summary>
        /// <param name="file"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        [Obsolete]
        [HttpPost("picture/upload")]
        public IActionResult UploadFile(IFormFile file,                         //文件对象
                                    [FromServices] IWebHostEnvironment env)  //局部注入主机环境对象
        {
            //检查文件大小
            if (file.Length == 0)
            {
                return BadRequest(new { message = "未上传文件" });
            }
            //获取文件的MIME类型
            var mimeType = file.ContentType;
            // 定义一些常见的图片MIME类型  
            var imageMimeTypes = new List<string>
         {
             "image/jpeg",
             "image/png",
             "image/webp"
             // 可以根据需要添加其他图片MIME类型
             //,"image/bmp"
             //,"image/tiff"
             //, 
         };

            // 检查文件的MIME类型是否在图片MIME类型列表中  
            if (!imageMimeTypes.Contains(mimeType))
            {
                return BadRequest(new { message = "上传的文件不是图片" });
            }

            //拼接上传的文件路径
            string fileExt = Path.GetExtension(file.FileName);         //获取文件扩展名
            string fileName = Guid.NewGuid().ToString("N") + fileExt;  //生成全球唯一文件名
            string relPath = Path.Combine(@"\uploads", fileName);       //拼接相对路径
            string fullPath = Path.Combine(env.ContentRootPath, "Uploads", fileName);  //拼接绝对路径

            //创建文件
            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(fs); //把上传的文件file拷贝到fs里
                fs.Flush();      //刷新fs文件缓存区
            };

            //返回上传后的 相对路径
            return Ok(new { Data = relPath.Replace("\\", "/"), message = "上传图片成功" });
        }
    }
}
