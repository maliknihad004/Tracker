public class CrawlRun
{
    public int CrawlRunId { get; set; }
    public int MerchantId { get; set; }

    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }

    public string StatusScope { get; set; } = "crawl_run";
    public string StatusCode { get; set; } = "pending";
    public int ProductsFound { get; set; }
    public string? ErrorMessage { get; set; }

    public Merchant Merchant { get; set; } = null!;
    public Status Status { get; set; } = null!;

    public ICollection<CrawledListing> Listings { get; set; }
        = new List<CrawledListing>();
}
