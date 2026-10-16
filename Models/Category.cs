
public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Component> Components { get; set; }
        = new List<Component>();
}