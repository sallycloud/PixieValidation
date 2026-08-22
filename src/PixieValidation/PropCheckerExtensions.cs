using System.Runtime.CompilerServices;

namespace PixieValidation;

public static class PropValidatorExtensions
{
    /// <summary>
    /// Combines two checkers so both must pass. Returns the first error encountered.
    /// </summary>
    public static PropChecker<T> And<T>(this PropChecker<T> first, PropChecker<T> second) =>
        toValidate => first(toValidate) ?? second(toValidate);

    /// <summary>
    /// Skips the checker when the value is <see langword="null"/>; otherwise applies it.
    /// For reference types.
    /// </summary>
    public static PropChecker<T?> OptionalRef<T>(this PropChecker<T> checker) where T : class =>
        toValidate => toValidate is null ? null : checker(toValidate);

    /// <summary>
    /// Skips the checker when the value has no value; otherwise applies it.
    /// For value types.
    /// </summary>
    public static PropChecker<T?> OptionalVal<T>(this PropChecker<T> checker) where T : struct =>
        toValidate => toValidate.HasValue ? checker(toValidate.Value) : null;

    /// <summary>
    /// Applies the checker only when <paramref name="condition"/> holds for the value.
    /// </summary>
    public static PropChecker<T> When<T>(this PropChecker<T> checker, Func<T, bool> condition) =>
        toValidate => condition(toValidate) ? checker(toValidate) : null;
    
    /// <summary>
    /// Checks <paramref name="toValidate"/> and throws a <see cref="ValidationException"/>
    /// if it is invalid.
    /// </summary>
    /// <returns><paramref name="toValidate"/>, if validation succeeds.</returns>
    public static T ValidateOrThrow<T>(
        this PropChecker<T> checker,
        T toValidate,
        [CallerArgumentExpression(nameof(toValidate))] string? propertyName = null)
    {
        var error = checker(toValidate);
        if (error is not null)
            throw new ValidationException([new ValidationError(propertyName!, error)]);
        return toValidate;
    }
    
    /// <summary>
    /// Checks each element of <paramref name="values"/> and throws a <see cref="ValidationException"/>
    /// if any element is invalid. Unlike checking a single value, all invalid elements are collected;
    /// the exception contains one <see cref="ValidationError"/> per invalid element, with the element's
    /// index appended to its path (e.g. <c>usernames[2]</c>).
    /// </summary>
    /// <returns><paramref name="values"/>, if every element is valid.</returns>
    public static IReadOnlyList<T> ValidateOrThrow<T>(
        this PropChecker<T> checker,
        IReadOnlyList<T> values,
        [CallerArgumentExpression(nameof(values))] string? basePath = null)
    {
        var errors = CollectionValidationHelper.CollectErrors(values, checker, basePath!).ToList();
        if (errors.Count > 0)
            throw new ValidationException(errors);
        return values;
    }

    /// <summary>
    /// Checks each element of <paramref name="values"/> and throws a <see cref="ValidationException"/>
    /// if any element is invalid. Unlike checking a single value, all invalid elements are collected;
    /// the exception contains one <see cref="ValidationError"/> per invalid element, with the element's
    /// iteration index appended to its path (e.g. <c>usernames[2]</c>). Since sets have no guaranteed
    /// order, this index may not be stable across calls.
    /// </summary>
    /// <returns><paramref name="values"/>, if every element is valid.</returns>
    public static IReadOnlySet<T> ValidateOrThrow<T>(
        this PropChecker<T> checker,
        IReadOnlySet<T> values,
        [CallerArgumentExpression(nameof(values))] string? basePath = null)
    {
        var errors = CollectionValidationHelper.CollectErrors(values, checker, basePath!).ToList();
        if (errors.Count > 0)
            throw new ValidationException(errors);
        return values;
    }
}