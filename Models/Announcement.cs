using System;
using System.Collections.Generic;

namespace ShopBackgroundSystem.Models
{
    public partial class Announcement
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Detail { get; set; }
        public DateTime Time { get; set; }
        public ulong Isdeleted { get; set; }
        public string? Owner { get; set; }
        public int? Uid { get; set; }
    }
}
