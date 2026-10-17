public class Category
{
    public int CategoryId { get; set; }
    public int? ParentCategoryId { get; set; }
    public string Slug { get; set; } = null!;
    public string NameTranslationKey { get; set; } = null!;
    public string? DescriptionTranslationKey { get; set; }
    public bool IsActive { get; set; } = true;

    public Category? ParentCategory { get; set; }

    public ICollection<Category> ChildCategories { get; set; }
        = new List<Category>();

    public ICollection<ProductCategory> ProductCategories { get; set; }
        = new List<ProductCategory>();
}
