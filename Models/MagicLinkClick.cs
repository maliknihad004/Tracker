public class MagicLinkClick
{
    public int ClickId { get; set; }
    public int MagicLinkId { get; set; }
    public int OfferId { get; set; }

    public DateTimeOffset ClickedAt { get; set; }

    public MagicLink MagicLink { get; set; } = null!;
    public Offer Offer { get; set; } = null!;

    public ICollection<AffiliateConversion> AffiliateConversions { get; set; }
        = new List<AffiliateConversion>();
}