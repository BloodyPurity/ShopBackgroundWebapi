using System;
using System.Collections.Generic;

namespace ShopBackgroundSystem.Models
{
    public partial class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Goodstype { get; set; } = null!;
        public string Holder { get; set; } = null!;
    }
}
