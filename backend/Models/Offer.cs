using System.Text.Json;

public class Offer
{
    public int OfferId { get; set; }
    public int MerchantId { get; set; }
    public int VariantId { get; set; }

    public string? ExternalOfferId { get; set; }
    public string ProductUrl { get; set; } = null!;
    public string? AffiliateUrl { get; set; }

    public decimal Price { get; set; }
    public decimal? ShippingCost { get; set; }
    public string Currency { get; set; } = null!;
    public bool? IsAvailable { get; set; }
    public JsonDocument? Attributes { get; set; }

    public DateTimeOffset LastCheckedAt { get; set; }

    public Merchant Merchant { get; set; } = null!;
    public ProductVariant Variant { get; set; } = null!;

    public ICollection<PriceHistory> PriceHistories { get; set; }
        = new List<PriceHistory>();

    public ICollection<ShareLinkClick> ShareLinkClicks { get; set; }
        = new List<ShareLinkClick>();

    public ICollection<AffiliateConversion> AffiliateConversions { get; set; }
        = new List<AffiliateConversion>();

    public ICollection<SponsoredListing> SponsoredListings { get; set; }
        = new List<SponsoredListing>();
}
