using System;
using System.Collections.Generic;

namespace ShopBackgroundSystem.Models
{
    public partial class User
    {
        public int Uid { get; set; }
        public string Uaccount { get; set; } = null!;
        public string Upwd { get; set; } = null!;
        public string Utype { get; set; } = null!;
        public string? Uname { get; set; }
        public string? Usex { get; set; }
        public string? Uadress { get; set; }
        public decimal? Usalary { get; set; }
        public DateTime? Ubirth { get; set; }
        public string? Uphone { get; set; }
        public string? Uicon { get; set; }
    }
}
