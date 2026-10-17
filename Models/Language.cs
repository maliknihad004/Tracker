public class Language
{
    public int LanguageId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public ICollection<Translation> Translations { get; set; }
        = new List<Translation>();

    public ICollection<User> Users { get; set; }
        = new List<User>();
}
