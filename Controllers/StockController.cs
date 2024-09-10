using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBackgroundSystem.Extensions;
using ShopBackgroundSystem.Helpers;
using ShopBackgroundSystem.Models;
using ShopBackgroundSystem.VM;

namespace ShopBackgroundSystem.Controllers
{
    [Route("1.0/[controller]")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        private CustomerDbContext _customerDbContext;
        public StockController(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }
        //由于库房操作记录量可能会很大，所以直接在数据库中分页------------------------------------------------------
        [AuthRole(requiredrole: new string[] { "admin" })]
        [HttpGet("getallstockin/{pageSize}-{pageIndex}")]
        public async Task<ActionResult<IEnumerable<Goodsstockinlog>>> AllStockin(int pageIndex, int pageSize)
        {
            var nocheckedcountin = await _customerDbContext.Goodsstockinlogs.CountAsync(l => l.Ischecked == 0);
            var TotalCount = await _customerDbContext.Goodsstockinlogs.CountAsync();
            int TotalPage = (int)Math.Ceiling((double)TotalCount / pageSize);
            if (pageIndex > TotalPage)
                pageIndex = TotalPage;
            if (pageIndex < 1)
                pageIndex = 1;
            //优化分页查询速度，先查ID，再通过ID查数据。
            var idlist = await _customerDbContext.Goodsstockinlogs.AsNoTracking().OrderByDescending(i => i.Time)
                .Skip(pageIndex * pageSize - pageSize).Take(pageSize)
                .Select(i=>i.Id).ToListAsync();
            var list = await _customerDbContext.Goodsstockinlogs.AsNoTracking().Where(i => idlist.Contains(i.Id))
                .OrderByDescending(i => i.Time).ToListAsync();
            var result = new
            {
                nocheckedcountin,
                TotalCount,
                TotalPage,
                list
            };
            return Ok(result);
        }
        [AuthRole(requiredrole: new string[] { "admin" })]
        [HttpGet("getallstockout/{pageSize}-{pageIndex}")]
        public async Task<ActionResult<IEnumerable<Goodsstockinlog>>> AllStockout(int pageIndex, int pageSize)
        {
            var nocheckedcountout = await _customerDbContext.Goodsstockoutlogs.CountAsync(l => l.Ischecked == 0);
            var TotalCount = await _customerDbContext.Goodsstockoutlogs.CountAsync();
            int TotalPage = (int)Math.Ceiling((double)TotalCount / pageSize);
            if (pageIndex > TotalPage)
                pageIndex = TotalPage;
            if (pageIndex < 1)
                pageIndex = 1;
            var idlist = await _customerDbContext.Goodsstockoutlogs.AsNoTracking().OrderByDescending(i => i.Time)
                .Skip(pageIndex * pageSize - pageSize).Take(pageSize).Select(l=>l.Id).ToListAsync();
            var list = await _customerDbContext.Goodsstockoutlogs.AsNoTracking().Where(i => idlist.Contains(i.Id)).OrderByDescending(i => i.Time).ToListAsync();
            var result = new
            {
                nocheckedcountout,
                TotalCount,
                TotalPage,
                list
            };
            return Ok(result);
        }
        [HttpGet("getmystockin/{uaccount}/{pageSize}-{pageIndex}")]
        public async Task<ActionResult<IEnumerable<Goodsstockinlog>>> Stockin(string uaccount,int pageIndex, int pageSize)
        {
            var TotalCount = await _customerDbContext.Goodsstockinlogs.Where(r => r.Uaccount == uaccount).CountAsync();
            int TotalPage = (int)Math.Ceiling((double)TotalCount / pageSize);
            if (pageIndex > TotalPage)
                pageIndex = TotalPage;
            if (pageIndex < 1)
                pageIndex = 1;
            var idlist = await _customerDbContext.Goodsstockinlogs.AsNoTracking().OrderByDescending(i => i.Time).Where(r => r.Uaccount == uaccount)
                .Skip(pageIndex * pageSize - pageSize).Take(pageSize).Select(l=>l.Id).ToListAsync();
            var list = await _customerDbContext.Goodsstockinlogs.AsNoTracking().Where(i=>idlist.Contains(i.Id)).OrderByDescending(i=>i.Time).ToListAsync();
            var result = new
            {
                TotalCount,
                TotalPage,
                list
            };
            return Ok(result);
        }
        [HttpGet("getmystockout/{uaccount}/{pageSize}-{pageIndex}")]
        public async Task<ActionResult<IEnumerable<Goodsstockinlog>>> Stockout(string uaccount, int pageIndex, int pageSize)
        {
            var TotalCount = await _customerDbContext.Goodsstockoutlogs.Where(r => r.Uaccount == uaccount).CountAsync();
            int TotalPage = (int)Math.Ceiling((double)TotalCount / pageSize);
            if (pageIndex > TotalPage)
                pageIndex = TotalPage;
            if (pageIndex < 1)
                pageIndex = 1;
            var idlist = await _customerDbContext.Goodsstockoutlogs.AsNoTracking().OrderByDescending(i => i.Time).Where(r => r.Uaccount == uaccount)
                .Skip(pageIndex * pageSize - pageSize).Take(pageSize).Select(i=>i.Id).ToListAsync();
            var list = await _customerDbContext.Goodsstockoutlogs.AsNoTracking().Where(i => idlist.Contains(i.Id)).OrderByDescending(i => i.Time).ToListAsync();
            var result = new
            {
                TotalCount,
                TotalPage,
                list
            };
            return Ok(result);
        }
        //出入库需要的数据源------------------------------------------------------
        //商品类型
        [HttpGet("getType")]
        public async Task<ActionResult<IEnumerable<Goodstype>>> GetGType()
        {
            var list = await _customerDbContext.Goodstypes.OrderBy(g => g.Id).ToListAsync();
            return Ok(list);
        }
        //根据商品类型获取供应商信息 这里因为get请求不能带body所以换post
        [HttpPost("getproviderbytype")]
        public async Task<ActionResult<IEnumerable<Goodstype>>> GetProviderbyGType(GtypeVM gtypeVM)
        {
            var list = await _customerDbContext.Providers
                .Where(p => p.Goodstype == gtypeVM.newtype)
                .OrderBy(p => p.Id)
                .Select(g => new {g.Name})
                .ToListAsync();
            return Ok(list);
        }
        //根据商品类型获取商品信息
        [HttpPost("getgoodsbytype")]
        public async Task<ActionResult<IEnumerable<Goodstype>>> GetGoodsbyGType(GtypeVM gtypeVM)
        {
            var list = await _customerDbContext.Goods
                .Where(p => p.Gtype == gtypeVM.newtype && p.Isdeleted == 0)
                .OrderBy(p => p.Id)
                .Select(g => new { g.Gname })
                .ToListAsync();
            return Ok(list);
        }
        //库房入库操作记录增删改------------------------------------------------------
        #region 已经弃用的代码
        [Obsolete]
        [HttpPost("autotestaddstockin")]
        public async Task<ActionResult> AutoAddStockin()
        {
            Random rnd = new();
            var _goodsname = await _customerDbContext.Goods.Select(g => g.Gname).ToListAsync();
            var _provider = await _customerDbContext.Providers.Select(p => p.Name).ToListAsync();
            if (_provider != null)
            {
                for(int i = 0; i < 100000; i++)
                {
                    var datetime = DateTime.Now.AddDays(rnd.NextInt64(-200, -1)).AddSeconds(rnd.NextInt64(-3600, 3600));
                    var stockinlog = new Goodsstockinlog();
                    stockinlog.Gname = _goodsname.PickRandom();
                    stockinlog.Pname = _provider.PickRandom();
                    stockinlog.Uaccount = "admin";
                    stockinlog.Cost = 0;
                    stockinlog.Count = 0;
                    stockinlog.Notes = "";
                    stockinlog.Time = datetime;
                    stockinlog.Ischecked = 0;
                    await _customerDbContext.Goodsstockinlogs.AddAsync(stockinlog);
                }
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "记录添加成功！" });
            }
            return BadRequest(new { message = "不存在该商品或供应商！" });
        }
        #endregion
        [HttpPost("addstockin")]
        public async Task<ActionResult> AddStockin(StockInVM stockInVM)
        {
            var _good = await _customerDbContext.Goods.SingleOrDefaultAsync(g => g.Gname == stockInVM.Gname);
            var _provider = await _customerDbContext.Providers.FirstOrDefaultAsync(g => g.Name == stockInVM.Pname);
            if(_good != null && _provider != null)
            {
                var stockinlog = new Goodsstockinlog();
                stockinlog.Gname = stockInVM.Gname;
                stockinlog.Pname = stockInVM.Pname;
                stockinlog.Uaccount = stockInVM.Uaccount;
                stockinlog.Cost = stockInVM.Cost;
                stockinlog.Count = stockInVM.Count;
                stockinlog.Notes = stockInVM.Notes;
                stockinlog.Time = DateTime.Now;
                stockinlog.Ischecked = 0;
                await _customerDbContext.Goodsstockinlogs.AddAsync(stockinlog);
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "记录添加成功！" });
            }
            return BadRequest(new { message = "不存在该商品或供应商！" });
        }
        [HttpPut("changestockin")]
        public async Task<ActionResult> ChangeStockin(StockInChangeVM stockInChange)
        {
            var stockinlog = await _customerDbContext.Goodsstockinlogs.SingleOrDefaultAsync(s => s.Id == stockInChange.Id && s.Ischecked == 0);
            if (stockinlog != null)
            {
                stockinlog.Cost = stockInChange.Cost;
                stockinlog.Count = stockInChange.Count;
                stockinlog.Notes = stockInChange.Notes;
                stockinlog.Time = DateTime.Now;
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "记录修改成功！" });
            }
            return BadRequest(new { message = "该记录已被确定或不存在！" });
        }
        [HttpDelete("removestockin/{id}")]
        public async Task<ActionResult> RemoveStockin(int id)
        {
            var stockinlog = await _customerDbContext.Goodsstockinlogs.SingleOrDefaultAsync(s => s.Id == id && s.Ischecked == 0);
            if (stockinlog != null)
            {
                _customerDbContext.Goodsstockinlogs.Remove(stockinlog);
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "记录删除成功！" });
            }
            return BadRequest(new { message = "该记录已被确定或不存在！" });
        }
        //库房出库操作记录增删改------------------------------------------------------
        [HttpPost("addstockout")]
        public async Task<ActionResult> AddStockout(StockOutVM stockOutVM)
        {
            var _good = await _customerDbContext.Goods.SingleOrDefaultAsync(g => g.Gname == stockOutVM.Gname);
            if (_good != null)
            {
                var price = _good.Price;
                var discount = _good.Discount;
                var stockoutlog = new Goodsstockoutlog();
                stockoutlog.Gname = stockOutVM.Gname;
                stockoutlog.Uaccount = stockOutVM.Uaccount;
                if (stockOutVM.Specialprice != null)
                {//特殊处理价格不打折
                    stockoutlog.Perprice = (double)stockOutVM.Specialprice;
                    stockoutlog.Price = (double)stockOutVM.Specialprice * stockOutVM.Count;
                }
                else
                {//不打折的商品discount为-1，所以取绝对值
                    stockoutlog.Perprice = (double)price;
                    stockoutlog.Price = (double)price * Math.Abs(discount) * stockOutVM.Count;
                }
                stockoutlog.Count = stockOutVM.Count;
                stockoutlog.Notes = stockOutVM.Notes;
                stockoutlog.Time = DateTime.Now;
                stockoutlog.Ischecked = 0;
                await _customerDbContext.Goodsstockoutlogs.AddAsync(stockoutlog);
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "记录添加成功！" });
            }
            return BadRequest(new { message = "不存在该商品或供应商！" });
        }
        [HttpPut("changestockout")]
        public async Task<ActionResult> ChangeStockout(StockOutChangeVM stockOutChange)
        {
            var stockoutlog = await _customerDbContext.Goodsstockoutlogs.SingleOrDefaultAsync(s => s.Id == stockOutChange.Id && s.Ischecked == 0);
            if (stockoutlog != null)
            {
                var gname = stockoutlog.Gname;
                var _good = await _customerDbContext.Goods.SingleOrDefaultAsync(g => g.Gname == gname);
                if (_good != null)
                {
                    var discount = _good.Discount;
                    var price = _good.Price;
                    if (stockOutChange.Specialprice != null)
                    {//特殊处理价格不打折
                        stockoutlog.Perprice = (double)stockOutChange.Specialprice;
                        stockoutlog.Price = (double)stockOutChange.Specialprice * stockOutChange.Count;
                    }
                    else
                    {//不打折的商品discount为-1，所以取绝对值
                        stockoutlog.Perprice = (double)price;
                        stockoutlog.Price = (double)price * Math.Abs(discount) * stockOutChange.Count;
                    }
                    stockoutlog.Count = stockOutChange.Count;
                    stockoutlog.Notes = stockOutChange.Notes;
                    stockoutlog.Time = DateTime.Now;
                    await _customerDbContext.SaveChangesAsync();
                    return Ok(new { message = "记录修改成功！" });
                }
                return BadRequest(new { message = "不存在该商品！" });
            }
            return BadRequest(new { message = "该记录已被确定或不存在！" });
        }
        [HttpDelete("removestockout/{id}")]
        public async Task<ActionResult> RemoveStockout(int id)
        {
            var stockoutlog = await _customerDbContext.Goodsstockoutlogs.SingleOrDefaultAsync(s => s.Id == id && s.Ischecked == 0);
            if (stockoutlog != null)
            {
                _customerDbContext.Goodsstockoutlogs.Remove(stockoutlog);
                await _customerDbContext.SaveChangesAsync();
                return Ok(new { message = "记录删除成功！" });
            }
            return BadRequest(new { message = "该记录已被确定或不存在！" });
        }
        //店长的库房记录确认操作，会联动其它模块生效。
        [HttpPut("recognizestockin/{id}")]
        public async Task<ActionResult> RecognizeStockIn(int id)
        {
            var _record = await _customerDbContext.Goodsstockinlogs.FindAsync(id);
            if(_record != null)
            {
               if( _record.Ischecked == 1)
                {
                    return BadRequest(new { message = "该条记录已被确认！" });
                }
                else
                {
                    //确认记录
                    _record.Ischecked = 1;
                    //修改商品数量
                    var _goods = await _customerDbContext.Goods.Where(g => g.Gname == _record.Gname).SingleAsync();
                    _goods.Gcount += _record.Count;
                    //添加一条账单记录
                    var assetRecord = new Assetrecord();
                    assetRecord.Records = _record.Cost;
                    assetRecord.Reason = "入库";
                    assetRecord.Time = DateTime.Now;
                    await _customerDbContext.Assetrecords.AddAsync(assetRecord);
                    //账户余额变更
                    var  asset = await _customerDbContext.Assets.FirstAsync();
                    asset.Money -= _record.Cost;
                    await _customerDbContext.SaveChangesAsync();
                    return Ok(new { message = "成功确认！" });
                }
            }
            return BadRequest(new { message = "数据不存在或已删除。"});
        }
        [HttpPut("recognizestockout/{id}")]
        public async Task<ActionResult> RecognizeStockOut(int id)
        {
            var _record = await _customerDbContext.Goodsstockoutlogs.FindAsync(id);
            if (_record != null)
            {
                if (_record.Ischecked == 1)
                {
                    return BadRequest(new { message = "该条记录已被确认！" });
                }
                else
                {
                    //确认记录
                    _record.Ischecked = 1;
                    //修改商品数量
                    var _goods = await _customerDbContext.Goods.Where(g => g.Gname == _record.Gname).SingleAsync();
                    _goods.Gcount -= _record.Count;
                    //添加一条账单记录
                    var assetRecord = new Assetrecord();
                    assetRecord.Records = _record.Price;
                    assetRecord.Reason = "出库";
                    assetRecord.Time = DateTime.Now;
                    await _customerDbContext.Assetrecords.AddAsync(assetRecord);
                    //账户余额变更
                    var asset = await _customerDbContext.Assets.FirstAsync();
                    asset.Money += _record.Price;
                    await _customerDbContext.SaveChangesAsync();
                    return Ok(new { message = "成功确认！" });
                }
            }
            return BadRequest(new { message = "数据不存在或已删除。" });
        }
    }
}
