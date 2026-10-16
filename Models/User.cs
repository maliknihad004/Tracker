public class User
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<PCBuild> PCBuilds { get; set; }
        = new List<PCBuild>();

    public ICollection<PriceAlert> PriceAlerts { get; set; }
        = new List<PriceAlert>();

    public ICollection<Notification> Notifications { get; set; }
        = new List<Notification>();
}