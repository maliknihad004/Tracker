public class Translation
{
    public int TranslationId { get; set; }
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
    public int LanguageId { get; set; }

    public Language Language { get; set; } = null!;
}
