namespace ShopBackgroundSystem.VM
{
    public class ProviderVM
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<string> Goodstypes { get; set; } = null!;
    }
}
