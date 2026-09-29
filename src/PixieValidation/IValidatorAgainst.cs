namespace PixieValidation;

/// <summary>
/// Implemented by external validators whose check of <typeparamref name="T"/> depends on a
/// value the validator does not hold itself (e.g. a value from the parent being validated).
/// Not meant to be called directly — use <see cref="ValidatorAgainstExtensions.ValidateAgainst{T,TAgainst}"/>
/// or <see cref="ValidatorAgainstExtensions.ValidateOrThrowAgainst{T,TAgainst}"/> instead.
/// Deliberately has no <see cref="Valid{T}"/> counterpart: only a type's own
/// <see cref="IValidatableAgainst{TAgainst}"/> can produce one indirectly, via its parent.
/// </summary>
public interface IValidatorAgainst<T, TAgainst>
{
    void ValidateAndCollectAgainst(T toValidate, ErrorCollector errors, TAgainst against);
}