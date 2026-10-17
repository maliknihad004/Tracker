using System.Text.Json;

public class Product
{
    public int ProductId { get; set; }
    public string NameTranslationKey { get; set; } = null!;
    public string? DescriptionTranslationKey { get; set; }
    public string? Brand { get; set; }
    public string? ImageUrl { get; set; }
    public JsonDocument? Specifications { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<ProductCategory> ProductCategories { get; set; }
        = new List<ProductCategory>();

    public ICollection<ProductVariant> Variants { get; set; }
        = new List<ProductVariant>();

    public ICollection<ShareLink> ShareLinks { get; set; }
        = new List<ShareLink>();
}
