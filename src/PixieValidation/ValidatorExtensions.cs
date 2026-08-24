using System.Runtime.CompilerServices;

namespace PixieValidation;

public static class ValidatorExtensions
{
    /// <summary>
    /// Validates <paramref name="toValidate"/> and returns the collected errors, if any.
    /// </summary>
    public static ErrorCollector Validate<T>(this IValidator<T> validator, T toValidate, ErrorCollector? errors = null)
    {
        errors ??= new ErrorCollector();
        validator.ValidateAndCollect(toValidate, errors);
        return errors;
    }

    /// <summary>
    /// Validates <paramref name="toValidate"/> and throws a <see cref="ValidationException"/>
    /// if any errors are found.
    /// </summary>
    /// <returns><paramref name="toValidate"/>, if validation succeeds.</returns>
    public static T ValidateOrThrow<T>(this IValidator<T> validator, T toValidate, ErrorCollector? errors = null)
    {
        errors ??= new ErrorCollector();
        validator.ValidateAndCollect(toValidate, errors);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return toValidate;
    }

    /// <summary>
    /// Validates each element of <paramref name="toValidate"/> using <paramref name="validator"/> and
    /// throws a <see cref="ValidationException"/> if any element is invalid. Unlike checking a single
    /// value, all invalid elements are collected; the exception contains one <see cref="ValidationError"/>
    /// per error found, with the element's index appended to its path (e.g. <c>people[1].Name</c>).
    /// </summary>
    /// <returns><paramref name="toValidate"/>, if every element is valid.</returns>
    public static IReadOnlyList<T> ValidateOrThrow<T>(
        this IValidator<T> validator,
        IReadOnlyList<T> toValidate,
        [CallerArgumentExpression(nameof(toValidate))]
        string? basePath = null)
    {
        var errors = new ErrorCollector();
        errors.Nested(validator, toValidate, basePath!);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return toValidate;
    }

    /// <summary>
    /// Validates each element of <paramref name="toValidate"/> using <paramref name="validator"/> and
    /// throws a <see cref="ValidationException"/> if any element is invalid. Unlike checking a single
    /// value, all invalid elements are collected; the exception contains one <see cref="ValidationError"/>
    /// per error found, with the element's iteration index appended to its path (e.g. <c>people[1].Name</c>).
    /// Since sets have no guaranteed order, this index may not be stable across calls.
    /// </summary>
    /// <returns><paramref name="toValidate"/>, if every element is valid.</returns>
    public static IReadOnlySet<T> ValidateOrThrow<T>(
        this IValidator<T> validator,
        IReadOnlySet<T> toValidate,
        [CallerArgumentExpression(nameof(toValidate))]
        string? basePath = null)
    {
        var errors = new ErrorCollector();
        errors.Nested(validator, toValidate, basePath!);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return toValidate;
    }
    
    /// <summary>
    /// Validates each value in <paramref name="toValidate"/> using <paramref name="validator"/>
    /// and returns the collected errors, if any.
    /// </summary>
    public static ErrorCollector Validate<TKey, TValue>(
        this IValidator<TValue> validator, IReadOnlyDictionary<TKey, TValue> toValidate, ErrorCollector? errors = null)
        where TKey : notnull
    {
        errors ??= new ErrorCollector();
        errors.Nested(validator, toValidate, "");
        return errors;
    }

    /// <summary>
    /// Validates each value in <paramref name="toValidate"/> using <paramref name="validator"/>
    /// and throws a <see cref="ValidationException"/> if any errors are found.
    /// </summary>
    /// <returns><paramref name="toValidate"/>, if validation succeeds.</returns>
    public static IReadOnlyDictionary<TKey, TValue> ValidateOrThrow<TKey, TValue>(
        this IValidator<TValue> validator, IReadOnlyDictionary<TKey, TValue> toValidate, ErrorCollector? errors = null)
        where TKey : notnull
    {
        errors ??= new ErrorCollector();
        errors.Nested(validator, toValidate, "");
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return toValidate;
    }
    
    /// <summary>
    /// Validates each item in <paramref name="toValidate"/> using <paramref name="validator"/>
    /// and returns the collected errors, if any.
    /// </summary>
    public static ErrorCollector Validate<T>(this IValidator<T> validator, IEnumerable<T> toValidate, ErrorCollector? errors = null)
    {
        var materialized = toValidate.ToList();
        errors ??= new ErrorCollector();
        errors.Nested(validator, materialized, "");
        return errors;
    }

    /// <summary>
    /// Validates each item in <paramref name="toValidate"/> using <paramref name="validator"/>
    /// and throws a <see cref="ValidationException"/> if any errors are found.
    /// </summary>
    /// <returns><paramref name="toValidate"/>, if validation succeeds.</returns>
    public static IEnumerable<T> ValidateOrThrow<T>(this IValidator<T> validator, IEnumerable<T> toValidate, ErrorCollector? errors = null)
    {
        var materialized = toValidate.ToList();
        errors ??= new ErrorCollector();
        errors.Nested(validator, materialized, "");
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return materialized;
    }
}