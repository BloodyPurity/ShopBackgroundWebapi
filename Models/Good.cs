using System;
using System.Collections.Generic;

namespace ShopBackgroundSystem.Models
{
    public partial class Good
    {
        public int Id { get; set; }
        public string Gname { get; set; } = null!;
        public string Gtype { get; set; } = null!;
        public double? Gcount { get; set; }
        public decimal? Price { get; set; }
        public double? Discount { get; set; }
        public string? Notes { get; set; }
        public string? Gicon { get; set; }
        public ulong Isdeleted { get; set; }
    }
}
