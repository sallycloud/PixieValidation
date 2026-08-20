namespace PixieValidation.Tests.Domain;

public partial record Group : IValidatable
{
    public static PropChecker<string> NameChecker = value =>
        string.IsNullOrWhiteSpace(value) ? "Name is required." : null;

    public void ValidateAndCollect(ErrorCollector errors)
    {
        errors.Check(Name, NameChecker);
        errors.Nested(Persons);
    }
}