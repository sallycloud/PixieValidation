namespace PixieValidation;

/// <summary>
/// Implemented by validators. Not meant to be called directly —
/// use <see cref="ValidatorExtensions.Validate{T}"/> or
/// <see cref="ValidatorExtensions.ValidateOrThrow{T}"/> instead.
/// </summary>
public interface IValidator<T>
{
    void ValidateAndCollect(T toValidate, ErrorCollector errors);
}