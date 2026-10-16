
public class PCBuild
{
    public int BuildId { get; set; }
    public int UserId { get; set; }

    public string Name { get; set; } = null!;
    public decimal? Budget { get; set; }
    public string Currency { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public User User { get; set; } = null!;

    public ICollection<PCBuildItem> Items { get; set; }
        = new List<PCBuildItem>();

    public ICollection<MagicLink> MagicLinks { get; set; }
        = new List<MagicLink>();
}