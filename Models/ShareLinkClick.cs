public class ShareLinkClick
{
    public int ClickId { get; set; }
    public int ShareLinkId { get; set; }
    public int OfferId { get; set; }
    public DateTimeOffset ClickedAt { get; set; }

    public ShareLink ShareLink { get; set; } = null!;
    public Offer Offer { get; set; } = null!;

    public ICollection<AffiliateConversion> AffiliateConversions { get; set; }
        = new List<AffiliateConversion>();
}
