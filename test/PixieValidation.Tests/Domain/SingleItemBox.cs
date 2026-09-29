namespace PixieValidation.Tests.Domain;

public record SingleItemBox : BoxBase, IValidatable
{
    public required Item Item { get; init; }

    public void ValidateAndCollect(ErrorCollector errors)
    {
        errors.NestedAgainst(Item, Capacity);
    }
}