using System.Text.Json;

public class ProductVariant
{
    public int VariantId { get; set; }
    public int ProductId { get; set; }
    public string VariantCode { get; set; } = null!;
    public string? Gtin { get; set; }
    public string? ImageUrl { get; set; }
    public JsonDocument? Attributes { get; set; }
    public bool IsActive { get; set; } = true;

    public Product Product { get; set; } = null!;

    public ICollection<Offer> Offers { get; set; }
        = new List<Offer>();

    public ICollection<SavedCollectionItem> CollectionItems { get; set; }
        = new List<SavedCollectionItem>();

    public ICollection<PriceAlert> PriceAlerts { get; set; }
        = new List<PriceAlert>();

    public ICollection<CrawledListing> CrawledListings { get; set; }
        = new List<CrawledListing>();
}
