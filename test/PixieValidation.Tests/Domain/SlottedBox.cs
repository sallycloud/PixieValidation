namespace PixieValidation.Tests.Domain;

public record SlottedBox : BoxBase, IValidatable
{
    public required IReadOnlyDictionary<string, Item> ItemsBySlot { get; init; }

    public void ValidateAndCollect(ErrorCollector errors)
    {
        errors.NestedAgainst(ItemsBySlot, Capacity);
    }
}