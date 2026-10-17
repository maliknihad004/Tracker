public class User
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int? PreferredLanguageId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public Language? PreferredLanguage { get; set; }

    public ICollection<SavedCollection> SavedCollections { get; set; }
        = new List<SavedCollection>();

    public ICollection<PriceAlert> PriceAlerts { get; set; }
        = new List<PriceAlert>();

    public ICollection<Notification> Notifications { get; set; }
        = new List<Notification>();
}
