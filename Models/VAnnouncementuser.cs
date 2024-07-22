using System;
using System.Collections.Generic;

namespace ShopBackgroundSystem.Models
{
    public partial class VAnnouncementuser
    {
        public int Announcementid { get; set; }
        public int Userid { get; set; }
        public string? Uname { get; set; }
        public string Name { get; set; } = null!;
        public string Uaccount { get; set; } = null!;
        public int Id { get; set; }
    }
}
