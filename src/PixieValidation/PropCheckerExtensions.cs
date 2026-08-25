using System.Runtime.CompilerServices;
using PixieValidation.PropCheckers;

namespace PixieValidation;

public static class PropCheckerExtensions
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
        IReadOnlyList<T>? values,
        [CallerArgumentExpression(nameof(values))] string? basePath = null)
    {
        var errors = CollectionValidationHelper.CollectErrors(values, checker, basePath!).ToList();
        if (errors.Count > 0)
            throw new ValidationException(errors);
        return values!;
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
        IReadOnlySet<T>? values,
        [CallerArgumentExpression(nameof(values))] string? basePath = null)
    {
        var errors = CollectionValidationHelper.CollectErrors(values, checker, basePath!).ToList();
        if (errors.Count > 0)
            throw new ValidationException(errors);
        return values!;
    }
    
    /// <summary>
    /// Combines two checkers so that the value is valid if either one succeeds.
    /// Neither checker's own error message survives; call <see cref="WithMessage{T}(OrChecker{T}, string)"/>
    /// to supply the message used when both fail.
    /// </summary>
    public static OrChecker<T> Or<T>(this PropChecker<T> first, PropChecker<T> second) =>
        new(value => first(value) is null || second(value) is null ? null : "Invalid value.");

    /// <summary>
    /// Extends an existing <see cref="OrChecker{T}"/> with one more alternative, so that the
    /// value is valid if any of the combined checkers succeeds.
    /// </summary>
    public static OrChecker<T> Or<T>(this OrChecker<T> first, PropChecker<T> second) =>
        new(value => first.Checker(value) is null || second(value) is null ? null : "Invalid value.");

    /// <summary>
    /// Combines two checkers so that the value is valid only if exactly one of them succeeds.
    /// Neither checker's own error message survives; call <see cref="WithMessage{T}(XorChecker{T}, string)"/>
    /// to supply the message used when both or neither succeed.
    /// </summary>
    public static XorChecker<T> Xor<T>(this PropChecker<T> first, PropChecker<T> second) =>
        new(value => (first(value) is null) ^ (second(value) is null) ? null : "Invalid value.");

    /// <summary>
    /// Inverts a checker: the value is valid if <paramref name="checker"/> fails, and invalid if it succeeds.
    /// The original checker's error message is discarded; call <see cref="WithMessage{T}(NotChecker{T}, string)"/>
    /// to supply the message used when the wrapped checker unexpectedly succeeds.
    /// </summary>
    public static NotChecker<T> Not<T>(this PropChecker<T> checker) =>
        new(value => checker(value) is not null ? null : "Invalid value.");

    /// <summary>
    /// Finalizes an <see cref="OrChecker{T}"/> into a usable <see cref="PropChecker{T}"/>, replacing
    /// the placeholder error with <paramref name="errorMessage"/>.
    /// </summary>
    public static PropChecker<T> WithMessage<T>(this OrChecker<T> or, string errorMessage) =>
        value => or.Checker(value) is null ? null : errorMessage;

    /// <summary>
    /// Finalizes a <see cref="XorChecker{T}"/> into a usable <see cref="PropChecker{T}"/>, replacing
    /// the placeholder error with <paramref name="errorMessage"/>.
    /// </summary>
    public static PropChecker<T> WithMessage<T>(this XorChecker<T> xor, string errorMessage) =>
        value => xor.Checker(value) is null ? null : errorMessage;

    /// <summary>
    /// Finalizes a <see cref="NotChecker{T}"/> into a usable <see cref="PropChecker{T}"/>, replacing
    /// the placeholder error with <paramref name="errorMessage"/>.
    /// </summary>
    public static PropChecker<T> WithMessage<T>(this NotChecker<T> not, string errorMessage) =>
        value => not.Checker(value) is null ? null : errorMessage;
    
    /// <summary>
    /// Adapts a <see cref="PropChecker{T}"/> for a non-nullable reference type to accept a
    /// nullable value, treating <c>null</c> as invalid. The counterpart to
    /// <see cref="OptionalRef{T}"/>, which treats <c>null</c> as valid.
    /// </summary>
    public static PropChecker<T?> RequiredRef<T>(this PropChecker<T> checker, string errorMessage = "Value is required.")
        where T : class =>
        value => value is null ? errorMessage : checker(value);
}