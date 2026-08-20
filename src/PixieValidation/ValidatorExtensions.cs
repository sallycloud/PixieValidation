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
    
    /// <summary>
    /// Validates <paramref name="toValidate"/> and throws a <see cref="ValidationException"/>
    /// if any errors are found. Unlike <see cref="ValidateOrThrow{T}"/>, wraps the result in a
    /// <see cref="Valid{T}"/> so that later code can require an already-validated instance
    /// through its signature alone.
    /// </summary>
    /// <returns>A <see cref="Valid{T}"/> wrapping <paramref name="toValidate"/>.</returns>
    public static Valid<T> ToValidOrThrow<T>(this IValidator<T> validator, T toValidate, ErrorCollector? errors = null)
    {
        errors ??= new ErrorCollector();
        validator.ValidateAndCollect(toValidate, errors);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return Valid<T>.Create(toValidate);
    }
}