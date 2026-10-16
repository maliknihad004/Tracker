
public class AffiliateConversion
{
    public int ConversionId { get; set; }
    public int RetailerId { get; set; }
    public int? OfferId { get; set; }
    public int? ClickId { get; set; }

    public string? ExternalConversionId { get; set; }
    public decimal? OrderAmount { get; set; }
    public decimal CommissionAmount { get; set; }

    public string Currency { get; set; } = null!;
    public string Status { get; set; } = "Pending";
    public DateTimeOffset ReportedAt { get; set; }

    public Retailer Retailer { get; set; } = null!;
    public Offer? Offer { get; set; }
    public MagicLinkClick? Click { get; set; }
}