using System.Text.Json;

public class CrawledListing
{
    public int ListingId { get; set; }
    public int CrawlRunId { get; set; }
    public int? MatchedVariantId { get; set; }

    public string ProductName { get; set; } = null!;
    public string? ExternalOfferId { get; set; }
    public string ProductUrl { get; set; } = null!;

    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public JsonDocument? RawSpecifications { get; set; }

    public string StatusScope { get; set; } = "listing_match";
    public string StatusCode { get; set; } = "pending";
    public DateTimeOffset CrawledAt { get; set; }

    public CrawlRun CrawlRun { get; set; } = null!;
    public ProductVariant? MatchedVariant { get; set; }
    public Status Status { get; set; } = null!;
}
