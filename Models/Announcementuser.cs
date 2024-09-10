using System;
using System.Collections.Generic;

namespace ShopBackgroundSystem.Models
{
    public partial class Announcementuser
    {
        public int Rid { get; set; }
        public int Announcementid { get; set; }
        public string Uaccount { get; set; } = null!;
        public ulong Isconfirmed { get; set; }
    }
}
