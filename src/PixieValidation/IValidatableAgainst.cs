namespace PixieValidation;

/// <summary>
/// Implemented by types whose validity depends on a value they do not hold themselves
/// (e.g. a child that must lie within its parent's quarter). Not meant to be called directly;
/// the parent passes the value on via <see cref="ErrorCollector"/>. Deliberately has no
/// <see cref="Valid{T}"/> counterpart: only the parent can produce one.
/// </summary>
public interface IValidatableAgainst<TAgainst>
{
    void ValidateAndCollectAgainst(ErrorCollector errors, TAgainst against);
}