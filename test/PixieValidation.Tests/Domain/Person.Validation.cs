namespace PixieValidation.Tests.Domain;

public partial record Person : IValidatable
{
    public static Validator<string> NameValidator = value =>
        string.IsNullOrWhiteSpace(value) ? "Name is required." : null;

    public static Validator<int> AgeValidator = value =>
        value is < 0 or > 150 ? "Age must be between 0 and 150." : null;
    
    public void Validate(ErrorCollector errors)
    {
        errors.Check(Name, NameValidator);
        errors.Check(Age, AgeValidator);
    }
}