
public class Notification
{
    public int NotificationId { get; set; }
    public int UserId { get; set; }
    public int? AlertId { get; set; }

    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public PriceAlert? Alert { get; set; }
}