public class SavedCollectionItem
{
    public int CollectionItemId { get; set; }
    public int CollectionId { get; set; }
    public int VariantId { get; set; }
    public int Quantity { get; set; } = 1;

    public SavedCollection Collection { get; set; } = null!;
    public ProductVariant Variant { get; set; } = null!;
}
