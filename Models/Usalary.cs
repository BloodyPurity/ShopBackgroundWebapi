using System;
using System.Collections.Generic;

namespace ShopBackgroundSystem.Models
{
    public partial class Usalary
    {
        public int Id { get; set; }
        public int Uid { get; set; }
        public double Salary { get; set; }
        public DateTime Time { get; set; }
        public int? Countdown { get; set; }
    }
}
