public class ShareLink
{
    public int ShareLinkId { get; set; }
    public int? ProductId { get; set; }
    public int? CollectionId { get; set; }
    public string Slug { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public Product? Product { get; set; }
    public SavedCollection? Collection { get; set; }

    public ICollection<ShareLinkClick> Clicks { get; set; }
        = new List<ShareLinkClick>();
}
