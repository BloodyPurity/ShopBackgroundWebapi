namespace ShopBackgroundSystem.VM
{
    public class StockInVM
    {
        public string Gname { get; set; } = null!;
        public string Uaccount { get; set; } = null!;
        public string Pname { get; set; } = null!;
        public double Count { get; set; }
        public string? Notes { get; set; }
        public double Cost { get; set; }
    }
}
