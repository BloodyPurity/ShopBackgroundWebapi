using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBackgroundSystem.Helpers;
using ShopBackgroundSystem.Models;
using ShopBackgroundSystem.VM;

namespace ShopBackgroundSystem.Controllers
{
    [Authorize]
    [Route("1.0/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly CustomerDbContext _customerDbContext;
        public StoreController(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }
        #region 统计相关
        //yestoday
        [HttpGet("ystockincount")]
        public async Task<ActionResult> StockinCountY()
        {
            var day = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var counts =await _customerDbContext.Goodsstockinlogs
                .Where(g=>g.Ischecked==1)
                .GroupBy(x =>x.Time.ToString().Substring(0, 10))
                .Select(a => new
                {
                    Day = a.First().Time.ToString().Substring(0, 10),
                    Count = a.Count(),
                })
                .Where(a=> a.Day== day)
                .SingleOrDefaultAsync();
            if (counts == null)
            {
                counts = new
                {
                    Day = day,
                    Count = 0
                };
            }
            var res = new
            {
                day,//日期
                counts//进货笔数
            };
            return Ok(res);
        }
        [HttpGet("ystockoutcount")]
        public async Task<ActionResult> StockoutCountY()
        {
            var day = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var counts = await _customerDbContext.Goodsstockoutlogs
                .Where(g => g.Ischecked == 1)
                .GroupBy(x => x.Time.ToString().Substring(0, 10))
                .Select(a => new
                {
                    Day = a.First().Time.ToString().Substring(0, 10),
                    Count = a.Count(),
                })
                .Where(a => a.Day == day)
                .SingleOrDefaultAsync();
            if (counts == null)
            {
                counts = new
                {
                    Day = day,
                    Count = 0
                };
            }
            var res = new
            {
                day,//日期
                counts//进货笔数
            };
            return Ok(res);
        }
        //today
        [HttpGet("tstockincount")]
        public async Task<ActionResult> StockinCountT()
        {
            var day = DateTime.Now.ToString("yyyy-MM-dd");
            var counts = await _customerDbContext.Goodsstockinlogs
                .Where(g => g.Ischecked == 1)
                .GroupBy(x => x.Time.ToString().Substring(0, 10))
                .Select(a => new
                {
                    Day = a.First().Time.ToString().Substring(0, 10),
                    Count = a.Count(),
                })
                .Where(a => a.Day == day)
                .SingleOrDefaultAsync();
            if (counts == null)
            {
                counts = new
                {
                    Day = day,
                    Count = 0
                };
            }
            var res = new
            {
                day,//日期
                counts//进货笔数
            };
            return Ok(res);
        }
        [HttpGet("tstockoutcount")]
        public async Task<ActionResult> StockoutCountT()
        {
            var day = DateTime.Now.ToString("yyyy-MM-dd");
            var counts = await _customerDbContext.Goodsstockoutlogs
                .Where(g => g.Ischecked == 1)
                .GroupBy(x => x.Time.ToString().Substring(0, 10))
                .Select(a => new
                {
                    Day = a.First().Time.ToString().Substring(0, 10),
                    Count = a.Count(),
                })
                .Where(a => a.Day == day)
                .SingleOrDefaultAsync();
            if (counts == null)
            {
                counts = new
                {
                    Day = day,
                    Count = 0
                };
            }
            var res = new
            {
                day,//日期
                counts//进货笔数
            };
            return Ok(res);
        }
        [HttpGet("tstockincost")]
        public async Task<ActionResult> StockinCostT()
        {
            var day = DateTime.Now.ToString("yyyy-MM-dd");
            var costs = await _customerDbContext.Goodsstockinlogs
                .Where(g => g.Ischecked == 1)
                .GroupBy(x => x.Time.ToString().Substring(0, 10))
                .Select(a => new
                {
                    Day = a.First().Time.ToString().Substring(0, 10),
                    Cost = a.Select(b => b.Cost).Sum()
                })
                .Where(a => a.Day == day)
                .SingleOrDefaultAsync();
            if (costs == null)
            {
                costs = new
                {
                    Day = day,
                    Cost = 0.0
                };
            }
            var res = new
            {
                day,//日期
                costs
            };
            return Ok(res);
        }
        [HttpGet("tstockoutprice")]
        public async Task<ActionResult> StockoutPriceT()
        {
            var day = DateTime.Now.ToString("yyyy-MM-dd");
            var prices = await _customerDbContext.Goodsstockoutlogs
                .Where(g => g.Ischecked == 1)
                .GroupBy(x => x.Time.ToString().Substring(0, 10))
                .Select(a => new
                {
                    Day = a.First().Time.ToString().Substring(0, 10),
                    Price = a.Select(b=>b.Price).Sum()
                })
                .Where(a => a.Day == day)
                .SingleOrDefaultAsync();
            if (prices == null)
            {
                prices = new
                {
                    Day = day,
                    Price = 0.0
                };
            }
            var res = new
            {
                day,//日期
                prices//进货笔数
            };
            return Ok(res);
        }
        //month
        [HttpGet("mstockincost")]
        public async Task<ActionResult> StockinCostM()
        {
            var day = DateTime.Now.AddDays(-29).ToString("yyyy-MM-dd");
            var TimeDay = Convert.ToDateTime(day);
            var allcosts = await _customerDbContext.Goodsstockinlogs
                .Where(g => g.Ischecked == 1 &&g.Time >= TimeDay)
                .Select(a => new
                {
                    a.Cost
                }).ToListAsync();
            double cost = 0.0;
            int count = allcosts.Count();
            foreach (var c in allcosts)
            {
                cost += c.Cost;
            }
            var res = new
            {
                day,//日期
                count,
                cost
            };
            return Ok(res);
        }
        [HttpGet("mstockoutprice")]
        public async Task<ActionResult> StockoutPriceM()
        {
            var day = DateTime.Now.AddDays(-29).ToString("yyyy-MM-dd");
            var TimeDay = Convert.ToDateTime(day);
            var allprices = await _customerDbContext.Goodsstockoutlogs
                .Where(g => g.Ischecked == 1 && g.Time >= TimeDay)
                .Select(a => new
                {
                    a.Price
                }).ToListAsync();
            double price = 0.0;
            int count = allprices.Count();
            foreach (var c in allprices)
            {
                price += c.Price;
            }
            var res = new
            {
                day,//日期
                count,
                price
            };
            return Ok(res);
        }
        [HttpGet("mstockincostbygtype")]
        public async Task<ActionResult> StockinCostbyGtype()
        {
            var day = DateTime.Now.AddDays(-29).ToString("yyyy-MM-dd");
            var TimeDay = Convert.ToDateTime(day);
            var cbg = await _customerDbContext.Goodsstockinlogs
                .Join(_customerDbContext.Goods,
                        a => a.Gname,
                        b => b.Gname,
                        (a, b) => new 
                        { 
                            a.Time, 
                            a.Cost, 
                            b.Gtype 
                        })
                .Where(c=>c.Time>= TimeDay)
                .GroupBy(c => c.Gtype)
                .Select(g => new
                {
                    gtype = g.First().Gtype,
                    allcost = g.Select(g => g.Cost).Sum()
                })
                .ToListAsync();
            return Ok(cbg);
        }
        [HttpGet("mstockoutpricebygtype")]
        public async Task<ActionResult> StockoutPricebyGtype()
        {
            var day = DateTime.Now.AddDays(-29).ToString("yyyy-MM-dd");
            var TimeDay = Convert.ToDateTime(day);
            var cbg = await _customerDbContext.Goodsstockoutlogs
                .Join(_customerDbContext.Goods,
                        a => a.Gname,
                        b => b.Gname,
                        (a, b) => new 
                        { 
                            a.Time, 
                            a.Price, 
                            b.Gtype
                        })
                .Where(c => c.Time >= TimeDay)
                .GroupBy(c => c.Gtype)
                .Select(g => new 
                { 
                    gtype = g.First().Gtype, 
                    allprice = g.Select(g => g.Price).Sum() 
                })
                .ToListAsync();
            return Ok(cbg);
        }
        //按商品种类分开的各个商品的销量(入库量)及销售额(入库开销)统计。
        [HttpGet("mstockincostandcountbygoods")]
        public async Task<ActionResult> StockinCostandCountbyGoods()
        {
            var day = DateTime.Now.AddDays(-29).ToString("yyyy-MM-dd");
            var TimeDay = Convert.ToDateTime(day);
            var res =await _customerDbContext.Goodsstockinlogs
                .Join(_customerDbContext.Goods, a => a.Gname, b => b.Gname, (a, b) => new
                {
                    a.Time,
                    a.Gname,
                    a.Cost,
                    a.Count,
                    b.Gtype
                }).Where(s => s.Time >= TimeDay)
                .GroupBy(s => s.Gtype).Select(g => new
                {
                    g.Key,
                    goods = g.Select(c => new
                    {
                        c.Gname,
                        c.Cost,
                        c.Count
                    })
                    .GroupBy(c => c.Gname)
                    .Select(d => new
                    {
                        gname = d.Key,
                        cost = d.Select(e => e.Cost).Sum(),
                        count = d.Select(e => e.Count).Sum()
                    })
                })
                .ToListAsync();
            return Ok(res);
        }
        [HttpGet("mstockoutpriceandcountbygoods")]
        public async Task<ActionResult> StockoutPriceandCountbyGoods()
        {
            var day = DateTime.Now.AddDays(-29).ToString("yyyy-MM-dd");
            var TimeDay = Convert.ToDateTime(day);
            var res = await _customerDbContext.Goodsstockoutlogs
            .Join(_customerDbContext.Goods, a => a.Gname, b => b.Gname, (a, b) => new
            {
                a.Time,
                a.Gname,
                a.Price,
                a.Count,
                b.Gtype
            }).Where(s => s.Time >= TimeDay)
            .GroupBy(s => s.Gtype).Select(g => new
            {
                g.Key,
                goods = g.Select(c => new
                {
                    c.Gname,
                    c.Price,
                    c.Count
                })
                .GroupBy(c => c.Gname)
                .Select(d => new
                {
                    gname = d.Key,
                    price = d.Select(e => e.Price).Sum(),
                    count = d.Select(e => e.Count).Sum()
                })
            })
            .ToListAsync();
            /*.GroupBy(s =>new { s.Gtype,s.Gname }).Select(g => new
            {
                g.Key.Gtype,
                g.Key.Gname,
                goods = new
                {
                    price = g.Select(g => g.Price).Sum(),
                    count = g.Select(g => g.Count).Sum()
                }
            })
            .ToListAsync();*/
            return Ok(res);
        }
        #endregion
        #region 公告相关
        [HttpGet("getannouncementinfo/{uaccount}")]
        public async Task<ActionResult> Announcement(string uaccount)
        {
            var announcements = await _customerDbContext.Announcements
                                            .Where(a => a.Isdeleted == 0)
                                            .OrderByDescending(a => a.Time)
                                            .Select(a => new {a.Id,a.Name,a.Time})
                                            .ToListAsync();
            //找到公告中涉及到自己的、且没有确认的内容
            var called = await _customerDbContext.Announcementusers
                                            .Where(a => a.Uaccount == uaccount && a.Isconfirmed == 0)
                                            .Select(a => new { a.Announcementid })
                                            .ToListAsync();
            var res = new
            {
                announcements,
                called
            };
            return Ok(res);
        }
        [HttpGet("getannouncementdetail/{id}")]
        public async Task<ActionResult> DetailAnnouncement(int id)
        {
            var res = await _customerDbContext.Announcements.SingleOrDefaultAsync(a => a.Id == id && a.Isdeleted == 0);
            if (res != null)
            {
                return Ok(res);
            }
            return BadRequest(new { message = "该条目不存在或已删除" });
        }
        /// <summary>
        /// 向公告表中插入数据，并向公告-提醒用户表中插入该数据。
        /// </summary>
        /// <param name="announcement"></param>
        /// <returns></returns>
        [HttpPost("addannouncement")]
        public async Task<ActionResult> AddAnnouncement(AnnouncementVM announcement)
        {
            Announcement _announcement = new Announcement
            {
                Name = announcement.Name,
                Time = DateTime.Now,
                Detail = announcement.Detail,
                Owner = announcement.Owner,
                Isdeleted = 0,
                Uid = 0
            };
            //启用事务
            using var transaction = await _customerDbContext.Database.BeginTransactionAsync();
            try
            {
                //还原点
                await transaction.CreateSavepointAsync("BeforeAsave");
                await _customerDbContext.Announcements.AddAsync(_announcement);
                await _customerDbContext.SaveChangesAsync();
                if (announcement.At != null && announcement.At.Count != 0)
                {
                    var announcement1 = await _customerDbContext.Announcements.OrderByDescending(a => a.Time).FirstOrDefaultAsync(a => a.Name == _announcement.Name && a.Isdeleted == 0 && a.Owner == _announcement.Owner);
                    if (announcement1 != null)
                    {
                        int aid = announcement1.Id;
                        foreach (var item in announcement.At)
                        {
                            var announcementuser = new Announcementuser();
                            announcementuser.Announcementid = aid;
                            announcementuser.Uaccount = item;
                            announcementuser.Isconfirmed = 0;
                            await _customerDbContext.Announcementusers.AddAsync(announcementuser);
                        }
                        await _customerDbContext.SaveChangesAsync();
                    }
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackToSavepointAsync("BeforeAsave");
                return BadRequest(new { message = "可能发生了一些技术性故障" });
            }
            

            return Ok(new { message = "公告发布成功!" });
        }
        /// <summary>
        /// 假删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("deleteannouncement/{id}")]
        public async Task<ActionResult> DeleteAnnouncement(int id)
        {
            var res = await _customerDbContext.Announcements.FirstOrDefaultAsync(a => a.Isdeleted == 0 && a.Id == id);
            if (res != null)
            {
                res.Isdeleted = 1;
                await _customerDbContext.SaveChangesAsync();
            }
            return Ok(new { message = "成功删除!" });
        }
        [HttpPut("confirmannouncement/{uaccount}-{id}")]
        public async Task<ActionResult> ConfirmAnnouncement(int id,string uaccount)
        {
            var res = await _customerDbContext.Announcementusers.FirstOrDefaultAsync(a => a.Isconfirmed == 0 && a.Announcementid == id && a.Uaccount == uaccount);
            if (res != null)
            {
                res.Isconfirmed = 1;
                await _customerDbContext.SaveChangesAsync();
            }
            else return BadRequest(new { message = "无需确认" });
            return Ok(new { message = "成功确认!" });
        }
        #endregion
    }
}
