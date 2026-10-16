
public class Retailer
{
    public int RetailerId { get; set; }
    public string Name { get; set; } = null!;
    public string WebsiteUrl { get; set; } = null!;

    public bool SupportsAffiliateLinks { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Offer> Offers { get; set; }
        = new List<Offer>();

    public ICollection<CrawlRun> CrawlRuns { get; set; }
        = new List<CrawlRun>();

    public ICollection<AffiliateConversion> AffiliateConversions { get; set; }
        = new List<AffiliateConversion>();
}