
public class SponsoredListing
{
    public int SponsoredListingId { get; set; }
    public int OfferId { get; set; }

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    public decimal? SponsorshipFee { get; set; }
    public string Currency { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public Offer Offer { get; set; } = null!;
}