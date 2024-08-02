namespace ShopBackgroundSystem.VM
{
    public class GoodsVM
    {
        public string Gname { get; set; } = null!;
        public string Gtype { get; set; } = null!;
        public decimal Price { get; set; }
        public double Discount { get; set; }
        public string? Notes { get; set; }
        public string? Gicon { get; set; }
    }
    public class GoodsChangeVM
    {
        public string Gname { get; set; } = null!;
        public decimal Price { get; set; }
        public double Discount { get; set; }
        public string? Notes { get; set; }
        public string? Gicon { get; set; }
    }
}
