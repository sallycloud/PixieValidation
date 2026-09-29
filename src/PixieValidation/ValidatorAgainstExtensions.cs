using System.Runtime.CompilerServices;

namespace PixieValidation;

public static class ValidatorAgainstExtensions
{
    /// <summary>
    /// Validates <paramref name="toValidate"/> using <paramref name="validator"/> against
    /// <paramref name="against"/> and returns the collected errors, if any.
    /// </summary>
    public static ErrorCollector ValidateAgainst<T, TAgainst>(
        this IValidatorAgainst<T, TAgainst> validator,
        T toValidate,
        TAgainst against,
        ErrorCollector? errors = null)
    {
        errors ??= new ErrorCollector();
        validator.ValidateAndCollectAgainst(toValidate, errors, against);
        return errors;
    }

    /// <summary>
    /// Validates <paramref name="toValidate"/> using <paramref name="validator"/> against
    /// <paramref name="against"/> and throws a <see cref="ValidationException"/> if any errors
    /// are found.
    /// </summary>
    /// <returns><paramref name="toValidate"/>, if validation succeeds.</returns>
    public static T ValidateOrThrowAgainst<T, TAgainst>(
        this IValidatorAgainst<T, TAgainst> validator,
        T toValidate,
        TAgainst against,
        ErrorCollector? errors = null)
    {
        errors ??= new ErrorCollector();
        validator.ValidateAndCollectAgainst(toValidate, errors, against);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return toValidate;
    }

    /// <summary>
    /// Validates each value in <paramref name="toValidate"/> using <paramref name="validator"/>
    /// against <paramref name="against"/> and returns the collected errors, if any. Errors are
    /// prefixed with the argument expression and the entry's key (e.g. <c>items[Left].Size</c>).
    /// </summary>
    public static ErrorCollector ValidateAgainst<TKey, TValue, TAgainst>(
        this IValidatorAgainst<TValue, TAgainst> validator,
        IReadOnlyDictionary<TKey, TValue>? toValidate,
        TAgainst against,
        ErrorCollector? errors = null,
        [CallerArgumentExpression(nameof(toValidate))] string? basePath = null)
        where TKey : notnull
    {
        errors ??= new ErrorCollector();
        errors.NestedAgainst(validator, toValidate, against, basePath!);
        return errors;
    }

    /// <summary>
    /// Validates each value in <paramref name="toValidate"/> using <paramref name="validator"/>
    /// against <paramref name="against"/> and throws a <see cref="ValidationException"/> if any
    /// errors are found.
    /// </summary>
    /// <returns><paramref name="toValidate"/>, if validation succeeds.</returns>
    public static IReadOnlyDictionary<TKey, TValue> ValidateOrThrowAgainst<TKey, TValue, TAgainst>(
        this IValidatorAgainst<TValue, TAgainst> validator,
        IReadOnlyDictionary<TKey, TValue>? toValidate,
        TAgainst against,
        ErrorCollector? errors = null,
        [CallerArgumentExpression(nameof(toValidate))] string? basePath = null)
        where TKey : notnull
    {
        errors ??= new ErrorCollector();
        errors.NestedAgainst(validator, toValidate, against, basePath!);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return toValidate!;
    }

    /// <summary>
    /// Validates each item in <paramref name="toValidate"/> using <paramref name="validator"/>
    /// against <paramref name="against"/> and returns the collected errors, if any. Errors are
    /// prefixed with the argument expression and the item's index (e.g. <c>items[1].Size</c>).
    /// </summary>
    public static ErrorCollector ValidateAgainst<T, TAgainst>(
        this IValidatorAgainst<T, TAgainst> validator,
        IEnumerable<T>? toValidate,
        TAgainst against,
        ErrorCollector? errors = null,
        [CallerArgumentExpression(nameof(toValidate))] string? basePath = null)
    {
        errors ??= new ErrorCollector();
        if (toValidate is null)
        {
            errors.NestedAgainst(validator, null, against, basePath!);
            return errors;
        }
        var materialized = toValidate.ToList();
        errors.NestedAgainst(validator, materialized, against, basePath!);
        return errors;
    }

    /// <summary>
    /// Validates each item in <paramref name="toValidate"/> using <paramref name="validator"/>
    /// against <paramref name="against"/> and throws a <see cref="ValidationException"/> if any
    /// errors are found.
    /// </summary>
    /// <returns>The materialized items, if validation succeeds.</returns>
    public static IEnumerable<T> ValidateOrThrowAgainst<T, TAgainst>(
        this IValidatorAgainst<T, TAgainst> validator,
        IEnumerable<T>? toValidate,
        TAgainst against,
        ErrorCollector? errors = null,
        [CallerArgumentExpression(nameof(toValidate))] string? basePath = null)
    {
        errors ??= new ErrorCollector();
        if (toValidate is null)
        {
            errors.NestedAgainst(validator, null, against, basePath!);
            throw new ValidationException(errors.ToList());
        }
        var materialized = toValidate.ToList();
        errors.NestedAgainst(validator, materialized, against, basePath!);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return materialized;
    }
}