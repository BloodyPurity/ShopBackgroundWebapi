using System;
using System.Collections.Generic;

namespace ShopBackgroundSystem.Models
{
    public partial class Goodsstockoutlog
    {
        public int Id { get; set; }
        public string Gname { get; set; } = null!;
        public string Uaccount { get; set; } = null!;
        public double Count { get; set; }
        public string? Notes { get; set; }
        public DateTime Time { get; set; }
        public double Price { get; set; }
        public ulong Ischecked { get; set; }
        public double Perprice { get; set; }
    }
}
