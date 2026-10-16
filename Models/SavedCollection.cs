public class SavedCollection
{
    public int CollectionId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public User User { get; set; } = null!;

    public ICollection<SavedCollectionItem> Items { get; set; }
        = new List<SavedCollectionItem>();

    public ICollection<ShareLink> ShareLinks { get; set; }
        = new List<ShareLink>();
}
