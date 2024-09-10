using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ShopBackgroundSystem.Helpers;
using ShopBackgroundSystem.Models;
using ShopBackgroundSystem.Services.Implementations;
using ShopBackgroundSystem.Services.Interfaces;
using ShopBackgroundSystem.VM;
using System.Xml.Linq;

namespace ShopBackgroundSystem.Controllers
{
    [Authorize]
    [Route("1.0/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        private CustomerDbContext _customerDbContext;
        private IPictureService _pictureService;

        public GoodsController(CustomerDbContext customerDbContext, IPictureService pictureService)
        {
            _customerDbContext = customerDbContext;
            _pictureService = pictureService;
        }

        //商品类型
        [AuthRole(requiredrole: new string[] { "admin", "user" })]
        [HttpGet("getType")]
        public async Task<ActionResult<IEnumerable<Goodstype>>> GetGType()
        {
            var list = await _customerDbContext.Goodstypes.OrderBy(g=>g.Id).ToListAsync();
            return Ok(list);
        }
        /// <summary>
        /// 只提供名称
        /// </summary>
        /// <returns></returns>
        [AuthRole(requiredrole: new string[] { "admin", "user" })]
        [HttpGet("getTypes")]
        public async Task<ActionResult<IEnumerable<Goodstype>>> GetGTypes()
        {
            var list = await _customerDbContext.Goodstypes.OrderBy(g => g.Id).Select(g=>new { g.Name }).ToListAsync();
            return Ok(list);
        }
        [AuthRole(requiredrole: new string[] { "admin" })]
        [HttpPost("addType")]
        public async Task<ActionResult<IEnumerable<Goodstype>>> AddGType(GtypeVM gtypeVM)
        {
            if (!string.IsNullOrWhiteSpace(gtypeVM.newtype))
            {
                var goodstype = await _customerDbContext.Goodstypes.FirstOrDefaultAsync(t => t.Name == gtypeVM.newtype);
                if (goodstype != null)
                {
                    return BadRequest(new { message = "已存在该类别" });
                }
                Goodstype gtype = new Goodstype { Name = gtypeVM.newtype };
                await _customerDbContext.Goodstypes.AddAsync(gtype);
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "添加成功！" });
            }
            return BadRequest(new { message = "写点什么吧" });
        }

        [AuthRole(requiredrole: new string[] { "admin" })]
        [HttpDelete("removeType/{id}")]
        public async Task<ActionResult<IEnumerable<Goodstype>>> RemoveGType(int id)
        {
            var goodstype = await _customerDbContext.Goodstypes.FirstOrDefaultAsync(t => t.Id == id);
            if (goodstype != null)
            {
                _customerDbContext.Goodstypes.Remove(goodstype);
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "删除成功！" });
            }
            return BadRequest(new { message = "不存在此类型" });
        }

        //商品
        [AuthRole(requiredrole: new string[] { "admin", "user" })]
        [HttpGet("getGoodsOn/{pageSize}-{pageIndex}")]
        public async Task<ActionResult<IEnumerable<Good>>> GetGoodsOn(int pageIndex, int pageSize)
        {
            var list = await _customerDbContext.Goods
                .Where(g => g.Isdeleted == 0)
                .Select(n => new { n.Gname,n.Gicon,n.Gtype, n.Gcount, n.Price, n.Discount, n.Notes })
                .ToListAsync();
            int TotalCount = list.Count();
            int TotalPage = (int)Math.Ceiling((double)TotalCount / pageSize);
            if (pageIndex > TotalPage)
            {
                pageIndex = TotalPage;
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            var goods = list.Skip(pageIndex * pageSize - pageSize).Take(pageSize).ToList();
            var result = new
            {
                TotalCount,
                TotalPage,
                goods
            };
            return Ok(result);
        }
        [AuthRole(requiredrole: new string[] { "admin", "user" })]
        [HttpGet("getGoodsOnSingle")]
        public async Task<ActionResult<IEnumerable<Good>>> GetGoodsOnSingle(string Name)
        {   
            var res = await _customerDbContext.Goods
                .Where(g => g.Isdeleted == 0 && g.Gname.Contains(Name))
                .Select(n => new { n.Gname, n.Gicon, n.Gtype, n.Gcount, n.Price, n.Discount, n.Notes })
                .ToListAsync();
            if(res !=null)
                return Ok(res);
            return BadRequest(new { message = $"{Name}不存在或已下架"});
        }

        [AuthRole(requiredrole: new string[] { "admin", "user" })]
        [HttpGet("getGoodsOffSingle")]
        public async Task<ActionResult<IEnumerable<Good>>> GetGoodsOffSingle(string Name)
        {
            var res = await _customerDbContext.Goods
                .Where(g => g.Isdeleted == 1 && g.Gname == Name)
                .Select(n => new { n.Gname, n.Gicon, n.Gtype, n.Gcount, n.Price, n.Notes })
                .ToListAsync();
            if (res != null)
                return Ok(res);
            return BadRequest(new { message = $"{Name}不存在或在架上" });
        }

        [AuthRole(requiredrole: new string[] { "admin", "user" })]
        [HttpGet("getGoodsOff/{pageSize}-{pageIndex}")]
        public async Task<ActionResult<IEnumerable<Good>>> GetGoodsOff(int pageIndex, int pageSize)
        {
            var list = await _customerDbContext.Goods
                .Where(g => g.Isdeleted == 1)
                .Select(n => new { n.Gname, n.Gicon, n.Gtype, n.Gcount, n.Price, n.Notes })
                .ToListAsync();
            int TotalCount = list.Count();
            int TotalPage = (int)Math.Ceiling((double)TotalCount / pageSize);
            if (pageIndex > TotalPage)
            {
                pageIndex = TotalPage;
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            var goods = list.Skip(pageIndex * pageSize - pageSize).Take(pageSize).ToList();
            var result = new
            {
                TotalCount,
                TotalPage,
                goods
            };
            return Ok(result);
        }

        [AuthRole(requiredrole: new string[] { "admin" })]
        [HttpPut("letGoodsOnorOff")]
        public async Task<ActionResult<IEnumerable<Good>>> LetGoodsOnorOff(GoodsOnOffVM goods)
        {
            var good = await _customerDbContext.Goods.SingleOrDefaultAsync(g=>g.Gname == goods.Gname);
            if(good != null)
            {
                good.Isdeleted = (ulong)goods.to;
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "操作成功！" });
            }
            return BadRequest(new { message = "不存在此商品！" });
        }

        [AuthRole(requiredrole: new string[] { "admin", "user" })]
        [HttpPut("letGoodsDiscount")]
        public async Task<ActionResult<IEnumerable<Good>>> LetGoodsDiscount(GoodsDiscountVM goods)
        {
            if (!string.IsNullOrEmpty(goods.Gname) && string.IsNullOrEmpty(goods.Gtype))
            {
                var good = await _customerDbContext.Goods.SingleOrDefaultAsync(g => g.Gname == goods.Gname && g.Discount != -1);//discount为-1不许打折
                if (good != null)
                {
                    good.Discount = goods.discount;
                    await _customerDbContext.SaveChangesAsync();
                    return Ok(new { message = "操作成功！" });
                }
                return BadRequest(new { message = "此商品不允许打折！" });
            }
            else if (string.IsNullOrEmpty(goods.Gname) && !string.IsNullOrEmpty(goods.Gtype))
            {
                var _goods = await _customerDbContext.Goods.Where(g => g.Gtype == goods.Gtype && g.Discount!=-1).ToListAsync();
                
                if (_goods .Count>0)
                {
                    foreach(var good in _goods)
                    {
                        good.Discount = goods.discount;
                        Console.WriteLine(good.Gname);
                    }
                    await _customerDbContext.SaveChangesAsync();
                    return Ok(new { message = "操作成功！" });
                }
                return BadRequest(new { message = "此商品不允许打折！" });
            }
            return BadRequest(new { message = "此商品不存在！" });
        }

        [AuthRole(requiredrole: new string[] { "admin" })]
        [HttpPost("addGoods")]
        public async Task<ActionResult> AddGoods([FromForm] GoodsVM goods, IFormFile? file,[FromServices] IWebHostEnvironment env)
        {
            string? fileName = await _pictureService.GetPictureUrlAsync(file, env);
            var _good = await _customerDbContext.Goods.FirstOrDefaultAsync(g=>g.Gname == goods.Gname);
            var _gtype = await _customerDbContext.Goodstypes.FirstOrDefaultAsync(g => g.Name == goods.Gtype);
            if(_good == null && _gtype != null)
            {
                //AutoMapper?
                Good thisgood = new Good()
                {
                    Gname = goods.Gname,
                    Gicon = fileName,
                    Gtype = goods.Gtype,
                    Price = goods.Price,
                    Notes = goods.Notes,
                    Gcount = 0,
                    Discount = goods.Discount,//这个字段只有1或-1，用来判断商品能不能打折
                    Isdeleted = 0
                };
                await _customerDbContext.Goods.AddAsync(thisgood);
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "添加成功" });
            }
            return BadRequest(new {message = "商品已存在！"});
        }

        [HttpPost("changeGoods")]
        public async Task<ActionResult> ChangeGoods([FromForm] GoodsChangeVM goods, IFormFile? file, [FromServices] IWebHostEnvironment env)
        {
            string? fileName = await _pictureService.GetPictureUrlAsync(file, env);
            var _good = await _customerDbContext.Goods.FirstOrDefaultAsync(g => g.Gname == goods.Gname);
            if (_good != null )
            {
                _good.Price = goods.Price;
                _good.Notes = goods.Notes;
                if (_good.Discount == -1 || goods.Discount == -1)//如果原始状态为禁止打折或如果即将转换到的状态为允许打折，防止修改原始的折扣额
                    _good.Discount = goods.Discount;
                //图片修改
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (!string.IsNullOrEmpty(_good.Gicon))
                    {
                        string old_fileName = _good.Gicon;
                        string fileFullPath = Path.Combine(env.ContentRootPath, "Uploads", old_fileName);
                        if (System.IO.File.Exists(fileFullPath))
                        {
                            System.IO.File.Delete(fileFullPath);
                        }
                    }
                    _good.Gicon = fileName;
                }
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "修改成功" });
            }
            return BadRequest(new { message = "商品不存在！" });
        }

        [AuthRole(requiredrole: new string[] { "admin", "user" })]
        [HttpPost("addProvider")]
        public async Task<ActionResult> AddProvider(ProviderVM providerVM)
        {
            if (providerVM.Name != null)
            {
                foreach (var gtype in providerVM.Goodstypes)
                {
                    var _provider = await _customerDbContext.Providers.FirstOrDefaultAsync(p => p.Name == providerVM.Name && p.Goodstype == gtype);
                    if (_provider == null)
                    {
                        Provider provider = new Provider();
                        provider.Name = providerVM.Name;
                        provider.Goodstype = gtype;
                        provider.Description = providerVM.Description;
                        provider.Holder = "admin";
                        await _customerDbContext.Providers.AddAsync(provider);
                    }
                    await _customerDbContext.SaveChangesAsync();
                }
                return Ok(new { message = "添加成功" });
            }
            return BadRequest(new { message = "缺少名称" });
        }

        [AuthRole(requiredrole: new string[] { "admin", "user" })]
        [HttpGet("getProviderGtype")]
        public async Task<ActionResult> getProviderGtype()
        {
            var _provider =await _customerDbContext.Providers
                .GroupBy(pt =>pt.Name)
                .Select(ng => new
                {
                    Name = ng.First().Name,
                    Description = ng.First().Description,
                    Goodstype = string.Concat(ng.Select(ng=>ng.Goodstype + " "))
                }).ToListAsync() ;
                return Ok(_provider);
        }
        [AuthRole(requiredrole: new string[] { "admin", "user" })]
        [HttpDelete("removeProvider/{provider}")]
        public async Task<ActionResult> DeleteProvider(string provider)
        {
            var list = await _customerDbContext.Providers.Where(p => p.Name == provider).ToListAsync();
            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    _customerDbContext.Providers.Remove(item);
                }
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "删除成功" });
            }
            else return BadRequest(new { message = "不存在此条目" });
        }
    }
}
