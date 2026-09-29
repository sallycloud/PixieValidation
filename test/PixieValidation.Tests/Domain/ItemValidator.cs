using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.Domain;

public class ItemValidator : IValidatorAgainst<Item, int>
{
    public void ValidateAndCollectAgainst(Item toValidate, ErrorCollector errors, int capacity)
    {
        errors.Check(toValidate.Size, IntCheckers.Max(capacity), nameof(Item.Size));
    }
}