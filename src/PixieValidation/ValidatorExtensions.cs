namespace PixieValidation;

public static class ValidatorExtensions
{
    /// <summary>
    /// Validates <paramref name="toValidate"/> and returns the collected errors, if any.
    /// </summary>
    public static ErrorCollector Validate<T>(this IValidator<T> validator, T toValidate, ErrorCollector? errors = null)
    {
        errors ??= new ErrorCollector();
        validator.ValidateAndCollect(toValidate, errors);
        return errors;
    }

    /// <summary>
    /// Validates <paramref name="toValidate"/> and throws a <see cref="ValidationException"/>
    /// if any errors are found.
    /// </summary>
    /// <returns><paramref name="toValidate"/>, if validation succeeds.</returns>
    public static T ValidateOrThrow<T>(this IValidator<T> validator, T toValidate, ErrorCollector? errors = null)
    {
        errors ??= new ErrorCollector();
        validator.ValidateAndCollect(toValidate, errors);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return toValidate;
    }
}