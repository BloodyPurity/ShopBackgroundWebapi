using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using ShopBackgroundSystem.Helpers;
using ShopBackgroundSystem.Models;
using ShopBackgroundSystem.Services.Interfaces;
using ShopBackgroundSystem.VM;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ShopBackgroundSystem.Controllers
{
    [Authorize]
    [Route("1.0/[controller]")]
    [ApiController]
    public class EmploeeController : ControllerBase
    {
        private CustomerDbContext _customerDbContext;
        private readonly IUserService _userService;

        public EmploeeController(CustomerDbContext customerDbContext, IUserService userService)
        {
            _customerDbContext = customerDbContext;
            _userService = userService;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AuthRole(requiredrole: new string[] { "admin" })]
        [HttpGet("getall/{pageSize}-{pageIndex}")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllEmploee(int pageIndex,int pageSize)
        {
            //简单分页，一页显示pageSize条数据
            List<User> Emploees = await _customerDbContext.Users.Where(user => user.Utype == "user").OrderBy(user => user.Uid).ToListAsync();
            var listEmploees = Emploees.Where(u => u.Uid > 0).Select(s => new { s.Uaccount, s.Uname, s.Usex, s.Uadress, s.Ubirth, s.Uphone });
            int TotalEmploees = listEmploees.Count();
            int TotalPage = (int)Math.Ceiling((double)TotalEmploees / pageSize);
            if (pageIndex > TotalPage)
            {
                pageIndex = TotalPage;
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            var emploees  = listEmploees.Skip(pageIndex * pageSize-pageSize).Take(pageSize).ToList();
            var result = new
            {
                TotalEmploees,
                TotalPage,
                emploees
            };
            return Ok(result);
        }
        [HttpGet("getEname")]
        public async Task<ActionResult<IEnumerable<User>>> GetEname()
        {
            var Enames= await _customerDbContext.Users.Select(a=>a.Uname).ToListAsync();
            return Ok(Enames);
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="uaccount"></param>
        /// <returns></returns>
        [AuthRole(requiredrole: new string[] { "admin" })]
        [HttpPut("reset/{uaccount}")]
        public async Task<ActionResult> ResetPassword(string uaccount)
        {
            if (!string.IsNullOrEmpty(uaccount))
            {
                var _User = await _customerDbContext.Users.FirstOrDefaultAsync(u => u.Uaccount == uaccount);
                if (_User != null)
                {
                    _User.Upwd = MD5Helper.GetMD5(_User.Uaccount + "123456");
                    await _customerDbContext.SaveChangesAsync();
                    return Ok(new { message = "重置成功" });
                }
                return NotFound(new { message = "查无此人" });
            }
            return BadRequest(new {message = "这你是怎么查的？"}); 
        }
        /// <summary>
        /// 更改信息
        /// </summary>
        /// <param name="uaccount"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("changeinfo/{uaccount}")]
        [AuthRole(requiredrole: new string[] { "admin" })]
        public async Task<ActionResult> ChangeInfo(string uaccount,UserChangeVM user)
        {
            if(!string.IsNullOrEmpty(uaccount) && user != null)
            {
                if (uaccount == user.Uaccount)
                {
                    var _user = await _customerDbContext.Users.FirstOrDefaultAsync(u => u.Uaccount == uaccount);
                    if (_user != null)
                    {
                        _user.Uname = user.Uname;
                        _user.Usex = user.Usex;
                        _user.Uadress = user.Uadress;
                        _user.Uphone = user.Uphone;
                        if (user.Ubirth != null)
                        {
                            _user.Ubirth = user.Ubirth.Value.ToLocalTime();
                        }
                        await _customerDbContext.SaveChangesAsync();

                        return Ok(new { message = "成功修改" });
                    }
                    return NotFound(new { message = "查无此人" });
                }
                return BadRequest(new { message = "信息不符" });
            }
            return BadRequest(new { message = "不推荐直接访问接口" });
        }
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="uaccount"></param>
        /// <returns></returns>
        [HttpDelete("delete/{uaccount}")]
        [ AuthRole(requiredrole: new string[] { "admin" })]
        public async Task<ActionResult> DeleteUser(string uaccount)
        {
            if (!string.IsNullOrEmpty(uaccount))
            {
                var _User = await _customerDbContext.Users.FirstOrDefaultAsync(u => u.Uaccount == uaccount);
                if (_User != null)
                {
                    _customerDbContext.Users.Remove(_User);
                    await _customerDbContext.SaveChangesAsync();
                    return Ok(new { message = "删除成功" });
                }
                return NotFound(new { message = "查无此人" });
            }
            return BadRequest(new { message = "不推荐直接访问接口" });
        }
        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="uaccount"></param>
        /// <returns></returns>
        [HttpPost("newman/{uaccount}")]
        [AuthRole(requiredrole: new string[] { "admin" })]
        public async Task<ActionResult> AddUser(string uaccount)
        {
            if (!string.IsNullOrEmpty(uaccount))
            {
                var _User = await _customerDbContext.Users.FirstOrDefaultAsync(u => u.Uaccount == uaccount);
                if (_User != null)
                {
                    return BadRequest(new { message = "已存在账户！" });
                }
                else
                {
                    User user = new User { Uaccount = uaccount, Upwd = MD5Helper.GetMD5(uaccount+"123456"), Utype = "user" };
                    await _customerDbContext.Users.AddAsync(user);
                    await _customerDbContext.SaveChangesAsync();
                    return Ok(new { message = "添加成功" });
                }
            }
            return BadRequest(new { message = "不推荐直接访问接口" });
        }
    }
}
