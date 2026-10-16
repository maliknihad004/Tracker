public class CategoryTranslation
{
    public int CategoryId { get; set; }
    public string LanguageCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Category Category { get; set; } = null!;
    public Language Language { get; set; } = null!;
}
