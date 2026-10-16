
public class PCBuildItem
{
    public int BuildItemId { get; set; }
    public int BuildId { get; set; }
    public int ComponentId { get; set; }
    public int Quantity { get; set; } = 1;

    public PCBuild Build { get; set; } = null!;
    public Component Component { get; set; } = null!;
}