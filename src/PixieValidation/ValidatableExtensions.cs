namespace PixieValidation;

public static class ValidatableExtensions
{
    /// <summary>
    /// Validates <paramref name="validatable"/> and returns the collected errors, if any.
    /// </summary>
    public static ErrorCollector Validate<T>(this T validatable, ErrorCollector? errors = null) where T : IValidatable
    {
        errors ??= new ErrorCollector();
        validatable.ValidateAndCollect(errors);
        return errors;
    }
    
    /// <summary>
    /// Validates <paramref name="validatable"/> and throws a <see cref="ValidationException"/>
    /// if any errors are found. Unlike <see cref="ValidateOrThrow{T}"/>, wraps the result in a
    /// <see cref="Valid{T}"/> so that later code can require an already-validated instance
    /// through its signature alone.
    /// </summary>
    /// <returns>A <see cref="Valid{T}"/> wrapping <paramref name="validatable"/>.</returns>
    public static Valid<T> ToValidOrThrow<T>(this T validatable, ErrorCollector? errors = null) where T : IValidatable
    {
        errors ??= new ErrorCollector();
        validatable.ValidateAndCollect(errors);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return Valid<T>.Create(validatable);
    }

    /// <summary>
    /// Validates <paramref name="validatable"/> and throws a <see cref="ValidationException"/>
    /// if any errors are found.
    /// </summary>
    /// <returns><paramref name="validatable"/>, if validation succeeds.</returns>
    public static T ValidateOrThrow<T>(this T validatable, ErrorCollector? errors = null) where T : IValidatable
    {
        errors ??= new ErrorCollector();
        validatable.ValidateAndCollect(errors);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return validatable;
    }
}