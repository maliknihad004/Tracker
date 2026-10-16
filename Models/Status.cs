public class Status
{
    public string Scope { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string LabelEn { get; set; } = null!;
    public string LabelAr { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }

    public ICollection<AffiliateConversion> AffiliateConversions { get; set; }
        = new List<AffiliateConversion>();

    public ICollection<CrawlRun> CrawlRuns { get; set; }
        = new List<CrawlRun>();

    public ICollection<CrawledListing> CrawledListings { get; set; }
        = new List<CrawledListing>();
}
