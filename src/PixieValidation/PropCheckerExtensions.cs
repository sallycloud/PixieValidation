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
    /// if it is invalid. Unlike <see cref="ValidateOrThrow{T}"/>, wraps the result in a
    /// <see cref="Valid{T}"/> so that later code can require an already-validated instance
    /// through its signature alone.
    /// </summary>
    /// <returns>A <see cref="Valid{T}"/> wrapping <paramref name="toValidate"/>.</returns>
    public static Valid<T> ToValidOrThrow<T>(
        this PropChecker<T> checker,
        T toValidate,
        [CallerArgumentExpression(nameof(toValidate))] string? propertyName = null)
    {
        var error = checker(toValidate);
        if (error is not null)
            throw new ValidationException([new ValidationError(propertyName!, error)]);
        return Valid<T>.Create(toValidate);
    }
    
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
}