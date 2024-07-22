using System;
using System.Collections.Generic;

namespace ShopBackgroundSystem.Models
{
    public partial class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ulong Isincome { get; set; }
        public double Money { get; set; }
    }
}
