
public class PriceHistory
{
    public int PriceHistoryId { get; set; }
    public int OfferId { get; set; }

    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;
    public DateTimeOffset RecordedAt { get; set; }

    public Offer Offer { get; set; } = null!;
}