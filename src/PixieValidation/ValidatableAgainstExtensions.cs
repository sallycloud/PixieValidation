using System.Runtime.CompilerServices;

namespace PixieValidation;

public static class ValidatableAgainstExtensions
{
    /// <summary>
    /// Validates <paramref name="validatable"/> against <paramref name="against"/> and returns
    /// the collected errors, if any.
    /// </summary>
    public static ErrorCollector ValidateAgainst<T, TAgainst>(
        this T validatable, TAgainst against, ErrorCollector? errors = null)
        where T : IValidatableAgainst<TAgainst>
    {
        errors ??= new ErrorCollector();
        validatable.ValidateAndCollectAgainst(errors, against);
        return errors;
    }

    /// <summary>
    /// Validates <paramref name="validatable"/> against <paramref name="against"/> and throws a
    /// <see cref="ValidationException"/> if any errors are found. Deliberately returns the plain
    /// instance and no <see cref="Valid{T}"/>: the caller chose <paramref name="against"/>,
    /// so nothing about it can be guaranteed by type.
    /// </summary>
    /// <returns><paramref name="validatable"/>, if validation succeeds.</returns>
    public static T ValidateOrThrowAgainst<T, TAgainst>(
        this T validatable, TAgainst against, ErrorCollector? errors = null)
        where T : IValidatableAgainst<TAgainst>
    {
        errors ??= new ErrorCollector();
        validatable.ValidateAndCollectAgainst(errors, against);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return validatable;
    }
    
    /// <summary>
    /// Validates each item in <paramref name="validatables"/> against <paramref name="against"/>
    /// and returns the collected errors, if any. Errors are prefixed with the argument expression
    /// and the item's index (e.g. <c>items[1].Size</c>).
    /// </summary>
    public static ErrorCollector ValidateAgainst<T, TAgainst>(
        this IEnumerable<T>? validatables,
        TAgainst against,
        ErrorCollector? errors = null,
        [CallerArgumentExpression(nameof(validatables))] string? basePath = null)
        where T : IValidatableAgainst<TAgainst>
    {
        errors ??= new ErrorCollector();
        if (validatables is null)
        {
            errors.NestedAgainst((IEnumerable<T>?)null, against, basePath!);
            return errors;
        }
        var materialized = validatables.ToList();
        errors.NestedAgainst(materialized, against, basePath!);
        return errors;
    }

    /// <summary>
    /// Validates each item in <paramref name="validatables"/> against <paramref name="against"/>
    /// and throws a <see cref="ValidationException"/> if any errors are found.
    /// </summary>
    /// <returns>The materialized items, if validation succeeds.</returns>
    public static IEnumerable<T> ValidateOrThrowAgainst<T, TAgainst>(
        this IEnumerable<T>? validatables,
        TAgainst against,
        ErrorCollector? errors = null,
        [CallerArgumentExpression(nameof(validatables))] string? basePath = null)
        where T : IValidatableAgainst<TAgainst>
    {
        errors ??= new ErrorCollector();
        if (validatables is null)
        {
            errors.NestedAgainst((IEnumerable<T>?)null, against, basePath!);
            throw new ValidationException(errors.ToList());
        }
        var materialized = validatables.ToList();
        errors.NestedAgainst(materialized, against, basePath!);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return materialized;
    }
    
    /// <summary>
    /// Validates each value in <paramref name="validatables"/> against <paramref name="against"/>
    /// and returns the collected errors, if any. Errors are prefixed with the argument expression
    /// and the entry's key (e.g. <c>items[Left].Size</c>).
    /// </summary>
    public static ErrorCollector ValidateAgainst<TKey, TValue, TAgainst>(
        this IReadOnlyDictionary<TKey, TValue>? validatables,
        TAgainst against,
        ErrorCollector? errors = null,
        [CallerArgumentExpression(nameof(validatables))] string? basePath = null)
        where TValue : IValidatableAgainst<TAgainst>
        where TKey : notnull
    {
        errors ??= new ErrorCollector();
        errors.NestedAgainst(validatables, against, basePath!);
        return errors;
    }

    /// <summary>
    /// Validates each value in <paramref name="validatables"/> against <paramref name="against"/>
    /// and throws a <see cref="ValidationException"/> if any errors are found.
    /// </summary>
    /// <returns><paramref name="validatables"/>, if validation succeeds.</returns>
    public static IReadOnlyDictionary<TKey, TValue> ValidateOrThrowAgainst<TKey, TValue, TAgainst>(
        this IReadOnlyDictionary<TKey, TValue>? validatables,
        TAgainst against,
        ErrorCollector? errors = null,
        [CallerArgumentExpression(nameof(validatables))] string? basePath = null)
        where TValue : IValidatableAgainst<TAgainst>
        where TKey : notnull
    {
        errors ??= new ErrorCollector();
        errors.NestedAgainst(validatables, against, basePath!);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return validatables!;
    }
}