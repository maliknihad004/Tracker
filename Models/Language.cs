public class Language
{
    public string LanguageCode { get; set; } = null!;
    public string NativeName { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public ICollection<User> Users { get; set; }
        = new List<User>();
}
