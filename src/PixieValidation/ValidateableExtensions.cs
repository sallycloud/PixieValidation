namespace PixieValidation;

public static class ValidatableExtensions
{
    public static void ValidateOrThrow(this IValidatable validatable)
    {
        var errors = new ErrorCollector();
        validatable.Validate(errors);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
    }
}