namespace ShopBackgroundSystem.VM
{
    public class StockOutVM
    {
        public string Gname { get; set; } = null!;
        public string Uaccount { get; set; } = null!;
        public double Count { get; set; }
        public string? Notes { get; set; }
        public double? Specialprice { get; set; }
    }
}
