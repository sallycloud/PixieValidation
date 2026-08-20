namespace PixieValidation;

/// <summary>
/// Checks a single value against a rule.
/// </summary>
/// <returns><see langword="null"/> if the value is valid; otherwise, the error message.</returns>
public delegate string? PropChecker<in T>(T toValidate);

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
}