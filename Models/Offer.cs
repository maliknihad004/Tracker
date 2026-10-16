
public class Offer
{
    public int OfferId { get; set; }
    public int RetailerId { get; set; }
    public int ComponentId { get; set; }

    public string? ExternalProductId { get; set; }
    public string ProductUrl { get; set; } = null!;
    public string? AffiliateUrl { get; set; }

    public decimal Price { get; set; }
    public decimal? ShippingCost { get; set; }
    public string Currency { get; set; } = null!;
    public bool? IsAvailable { get; set; }

    public DateTimeOffset LastCheckedAt { get; set; }

    public Retailer Retailer { get; set; } = null!;
    public Component Component { get; set; } = null!;

    public ICollection<PriceHistory> PriceHistories { get; set; }
        = new List<PriceHistory>();

    public ICollection<MagicLinkClick> MagicLinkClicks { get; set; }
        = new List<MagicLinkClick>();

    public ICollection<AffiliateConversion> AffiliateConversions { get; set; }
        = new List<AffiliateConversion>();

    public ICollection<SponsoredListing> SponsoredListings { get; set; }
        = new List<SponsoredListing>();
}