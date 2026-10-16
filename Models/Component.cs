
public class Component
{
    public int ComponentId { get; set; }
    public int CategoryId { get; set; }

    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? ManufacturerPartNumber { get; set; }
    public string? Specifications { get; set; }
    public string? ImageUrl { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Category Category { get; set; } = null!;

    public ICollection<Offer> Offers { get; set; }
        = new List<Offer>();

    public ICollection<PCBuildItem> BuildItems { get; set; }
        = new List<PCBuildItem>();

    public ICollection<MagicLink> MagicLinks { get; set; }
        = new List<MagicLink>();

    public ICollection<PriceAlert> PriceAlerts { get; set; }
        = new List<PriceAlert>();

    public ICollection<CrawledListing> CrawledListings { get; set; }
        = new List<CrawledListing>();
}