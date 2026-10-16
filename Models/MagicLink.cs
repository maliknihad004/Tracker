
public class MagicLink
{
    public int MagicLinkId { get; set; }

    public int? ComponentId { get; set; }
    public int? BuildId { get; set; }

    public string Slug { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public Component? Component { get; set; }
    public PCBuild? Build { get; set; }

    public ICollection<MagicLinkClick> Clicks { get; set; }
        = new List<MagicLinkClick>();
}