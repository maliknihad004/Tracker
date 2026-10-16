public class Language
{
    public string LanguageCode { get; set; } = null!;
    public string NativeName { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public ICollection<CategoryTranslation> CategoryTranslations { get; set; }
        = new List<CategoryTranslation>();

    public ICollection<ProductTranslation> ProductTranslations { get; set; }
        = new List<ProductTranslation>();

    public ICollection<User> Users { get; set; }
        = new List<User>();
}
