using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.Domain;

public partial record Person : IValidatable
{
    public static PropChecker<string> NameChecker =
        StringCheckers.NotEmpty();

    private static PropChecker<int> AgeRangeChecker =
        IntCheckers.Range(0, 150);

    public static PropChecker<int?> AgeChecker =
        AgeRangeChecker.OptionalVal();

    public void ValidateAndCollect(ErrorCollector errors)
    {
        errors.Check(Name, NameChecker);
        errors.Check(Age, AgeChecker);
    }
}