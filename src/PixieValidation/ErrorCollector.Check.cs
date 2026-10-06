using System.Runtime.CompilerServices;

namespace PixieValidation;

public sealed partial class ErrorCollector
{
    /// <summary>
    /// Records <paramref name="error"/> under <paramref name="propertyName"/>, if not <see langword="null"/>.
    /// </summary>
    public void Check(string propertyName, string? error)
    {
        if (error is not null)
            _errors.Add(new ValidationError(BuildPath(propertyName), error));
    }

    /// <summary>
    /// Runs <paramref name="checker"/> against <paramref name="value"/> and records the error, if any.
    /// The property name is inferred from the calling expression unless given explicitly.
    /// </summary>
    public void Check<T>(
        T value,
        PropChecker<T> checker,
        [CallerArgumentExpression(nameof(value))]
        string? propertyName = null)
    {
        var error = checker(value);
        if (error is not null)
            _errors.Add(new ValidationError(BuildPath(propertyName!), error));
    }

    /// <summary>
    /// Runs <paramref name="checker"/> against each element of <paramref name="values"/> and records
    /// the errors, if any. Unlike <see cref="Check{T}(T, PropChecker{T}, string)"/>, all invalid elements
    /// are collected, with the element's iteration index appended to its path (e.g. <c>Names[1]</c>).
    /// The property name is inferred from the calling expression unless given explicitly.
    /// </summary>
    public void CheckAll<T>(
        IReadOnlyCollection<T>? values,
        PropChecker<T> checker,
        [CallerArgumentExpression(nameof(values))]
        string? basePath = null)
    {
        foreach (var error in CollectionValidationHelper.CollectErrors(values, checker, BuildPath(basePath!)))
            _errors.Add(error);
    }

    /// <summary>
    /// Runs <paramref name="checker"/> against <paramref name="value"/> and <paramref name="against"/>,
    /// and records the error, if any. The property name is inferred from the calling expression
    /// unless given explicitly.
    /// </summary>
    public void CheckAgainst<T, TAgainst>(
        T value,
        TAgainst against,
        PropAgainstChecker<T, TAgainst> checker,
        [CallerArgumentExpression(nameof(value))]
        string? propertyName = null)
    {
        var error = checker(value, against);
        if (error is not null)
            _errors.Add(new ValidationError(BuildPath(propertyName!), error));
    }
}
