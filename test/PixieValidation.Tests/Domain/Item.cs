using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.Domain;

public record Item : IValidatableAgainst<int>
{
    public required int Size { get; init; }

    public void ValidateAndCollectAgainst(ErrorCollector errors, int capacity)
    {
        errors.Check(Size, IntCheckers.Max(capacity));
    }
}