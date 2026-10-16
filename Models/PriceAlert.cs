
public class PriceAlert
{
    public int AlertId { get; set; }
    public int UserId { get; set; }
    public int ComponentId { get; set; }

    public decimal TargetPrice { get; set; }
    public string Currency { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastTriggeredAt { get; set; }

    public User User { get; set; } = null!;
    public Component Component { get; set; } = null!;

    public ICollection<Notification> Notifications { get; set; }
        = new List<Notification>();
}