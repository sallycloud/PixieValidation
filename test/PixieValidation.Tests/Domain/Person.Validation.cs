namespace PixieValidation.Tests.Domain;

public partial record Person : IValidatable
{
    public static PropChecker<string> NameChecker = value =>
        string.IsNullOrWhiteSpace(value) ? "Name is required." : null;

    private static PropChecker<int> AgeRangeChecker = value =>
        value is < 0 or > 150 ? "Age must be between 0 and 150." : null;

    public static PropChecker<int?> AgeChecker =
        AgeRangeChecker.OptionalVal();
    
    public void ValidateAndCollect(ErrorCollector errors)
    {
        errors.Check(Name, NameChecker);
        errors.Check(Age, AgeChecker);
    }
}