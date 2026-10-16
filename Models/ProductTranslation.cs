public class ProductTranslation
{
    public int ProductId { get; set; }
    public string LanguageCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Product Product { get; set; } = null!;
    public Language Language { get; set; } = null!;
}
