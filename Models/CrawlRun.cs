
public class CrawlRun
{
    public int CrawlRunId { get; set; }
    public int RetailerId { get; set; }

    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }

    public string Status { get; set; } = "Pending";
    public int ProductsFound { get; set; }
    public string? ErrorMessage { get; set; }

    public Retailer Retailer { get; set; } = null!;

    public ICollection<CrawledListing> Listings { get; set; }
        = new List<CrawledListing>();
}