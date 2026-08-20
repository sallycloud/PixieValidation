namespace PixieValidation;

/// <summary>
/// Implemented by types that can validate themselves. Not meant to be called directly —
/// use <see cref="ValidatableExtensions.Validate{T}"/> or
/// <see cref="ValidatableExtensions.ValidateOrThrow{T}"/> instead.
/// </summary>
public interface IValidatable
{
    void ValidateAndCollect(ErrorCollector errors);
}

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