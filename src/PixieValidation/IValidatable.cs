namespace PixieValidation;

/// <summary>
/// Implemented by types that can validate themselves. Not meant to be called directly —
/// use <see cref="ValiValidatableExtensionsidate{T}"/> or
/// <see cref="ValiValidatableExtensionsidateOrThrow{T}"/> instead.
/// </summary>
public interface IValidatable
{
    void ValidateAndCollect(ErrorCollector errors);
}