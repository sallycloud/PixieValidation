namespace PixieValidation.Tests.Domain;

public record MultiItemBox : BoxBase, IValidatable
{
    public required IReadOnlyList<Item> Items { get; init; }

    public void ValidateAndCollect(ErrorCollector errors)
    {
        errors.NestedAgainst(Items, Capacity);
    }
}