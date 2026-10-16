
public class CrawledListing
{
    public int ListingId { get; set; }
    public int CrawlRunId { get; set; }
    public int? ComponentId { get; set; }

    public string ProductName { get; set; } = null!;
    public string? ExternalProductId { get; set; }
    public string ProductUrl { get; set; } = null!;

    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public string? RawSpecifications { get; set; }

    public string MatchStatus { get; set; } = "Pending";
    public DateTimeOffset CrawledAt { get; set; }

    public CrawlRun CrawlRun { get; set; } = null!;
    public Component? Component { get; set; }
}