using System;
using System.Collections.Generic;

namespace ShopBackgroundSystem.Models
{
    public partial class Assetrecord
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public double Records { get; set; }
        public string? Reason { get; set; }
    }
}
