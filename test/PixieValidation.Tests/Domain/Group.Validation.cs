namespace PixieValidation.Tests.Domain;

public partial record Group : IValidatable
{
    public static Validator<string> NameValidator = value =>
        string.IsNullOrWhiteSpace(value) ? "Name is required." : null;

    public void Validate(ErrorCollector errors)
    {
        errors.Check(Name, NameValidator);
        errors.Nested(Persons);
    }
}