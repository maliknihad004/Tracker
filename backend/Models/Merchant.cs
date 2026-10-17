public class Merchant
{
    public int MerchantId { get; set; }
    public string Name { get; set; } = null!;
    public string? WebsiteUrl { get; set; }
    public bool SupportsAffiliateLinks { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Offer> Offers { get; set; }
        = new List<Offer>();

    public ICollection<AffiliateConversion> AffiliateConversions { get; set; }
        = new List<AffiliateConversion>();

    public ICollection<CrawlRun> CrawlRuns { get; set; }
        = new List<CrawlRun>();
}
