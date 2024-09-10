namespace ShopBackgroundSystem.VM
{
    public class AnnouncementVM
    {
        public string Name { get; set; } = null!;
        public string Detail { get; set; } = null!;
        public List<string>? At { get; set; }
        public string? Owner { get; set; }
    }
}
