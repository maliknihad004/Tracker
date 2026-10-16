public class AffiliateConversion
{
    public int ConversionId { get; set; }
    public int MerchantId { get; set; }
    public int? OfferId { get; set; }
    public int? ClickId { get; set; }

    public string? ExternalConversionId { get; set; }
    public decimal? OrderAmount { get; set; }
    public decimal CommissionAmount { get; set; }

    public string Currency { get; set; } = null!;
    public string StatusScope { get; set; } = "affiliate_conversion";
    public string StatusCode { get; set; } = "pending";
    public DateTimeOffset ReportedAt { get; set; }

    public Merchant Merchant { get; set; } = null!;
    public Offer? Offer { get; set; }
    public ShareLinkClick? Click { get; set; }
    public Status Status { get; set; } = null!;
}
