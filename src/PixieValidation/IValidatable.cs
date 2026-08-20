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