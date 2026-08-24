using System.Runtime.CompilerServices;

namespace PixieValidation;

public static class ValidatableExtensions
{
    /// <summary>
    /// Validates <paramref name="validatable"/> and returns the collected errors, if any.
    /// </summary>
    public static ErrorCollector Validate<T>(this T validatable, ErrorCollector? errors = null) where T : IValidatable
    {
        errors ??= new ErrorCollector();
        validatable.ValidateAndCollect(errors);
        return errors;
    }
    
    /// <summary>
    /// Validates <paramref name="validatable"/> and throws a <see cref="ValidationException"/>
    /// if any errors are found. Unlike <see cref="ValidateOrThrow{T}(T, ErrorCollector)"/>,
    /// wraps the result in a <see cref="Valid{T}"/> so that later code can require an already-validated
    /// instance through its signature alone.
    /// </summary>
    /// <returns>A <see cref="Valid{T}"/> wrapping <paramref name="validatable"/>.</returns>
    public static Valid<T> ToValidOrThrow<T>(this T validatable, ErrorCollector? errors = null) where T : IValidatable
    {
        errors ??= new ErrorCollector();
        validatable.ValidateAndCollect(errors);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return Valid<T>.Create(validatable);
    }

    /// <summary>
    /// Validates <paramref name="validatable"/> and throws a <see cref="ValidationException"/>
    /// if any errors are found.
    /// </summary>
    /// <returns><paramref name="validatable"/>, if validation succeeds.</returns>
    public static T ValidateOrThrow<T>(this T validatable, ErrorCollector? errors = null) where T : IValidatable
    {
        errors ??= new ErrorCollector();
        validatable.ValidateAndCollect(errors);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return validatable;
    }
    
    /// <summary>
    /// Validates each element of <paramref name="validatables"/> and throws a
    /// <see cref="ValidationException"/> if any element is invalid. Unlike checking a single
    /// <see cref="IValidatable"/>, all invalid elements are collected; the exception contains one
    /// <see cref="ValidationError"/> per error found, with the element's index appended to its path
    /// (e.g. <c>people[1].Name</c>).
    /// </summary>
    /// <returns><paramref name="validatables"/>, if every element is valid.</returns>
    public static IReadOnlyList<T> ValidateOrThrow<T>(
        this IReadOnlyList<T> validatables,
        [CallerArgumentExpression(nameof(validatables))] string? basePath = null)
        where T : IValidatable
    {
        var errors = new ErrorCollector();
        errors.Nested(validatables, basePath!);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return validatables;
    }

    /// <summary>
    /// Validates each element of <paramref name="validatables"/> and throws a
    /// <see cref="ValidationException"/> if any element is invalid. Unlike
    /// <see cref="ValidateOrThrow{T}(IReadOnlyList{T}, string)"/>, wraps the result in a
    /// <see cref="Valid{T}"/> so that later code can require an already-validated collection
    /// through its signature alone.
    /// </summary>
    /// <returns>A <see cref="Valid{T}"/> wrapping <paramref name="validatables"/>.</returns>
    public static Valid<IReadOnlyList<T>> ToValidOrThrow<T>(
        this IReadOnlyList<T> validatables,
        [CallerArgumentExpression(nameof(validatables))] string? basePath = null)
        where T : IValidatable
    {
        var errors = new ErrorCollector();
        errors.Nested(validatables, basePath!);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return Valid<IReadOnlyList<T>>.Create(validatables);
    }
    
    /// <summary>
    /// Validates each element of <paramref name="validatables"/> and throws a
    /// <see cref="ValidationException"/> if any element is invalid. Unlike checking a single
    /// <see cref="IValidatable"/>, all invalid elements are collected; the exception contains one
    /// <see cref="ValidationError"/> per error found, with the element's iteration index appended
    /// to its path (e.g. <c>people[1].Name</c>). Since sets have no guaranteed order, this index
    /// may not be stable across calls.
    /// </summary>
    /// <returns><paramref name="validatables"/>, if every element is valid.</returns>
    public static IReadOnlySet<T> ValidateOrThrow<T>(
        this IReadOnlySet<T> validatables,
        [CallerArgumentExpression(nameof(validatables))] string? basePath = null)
        where T : IValidatable
    {
        var errors = new ErrorCollector();
        errors.Nested(validatables, basePath!);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return validatables;
    }

    /// <summary>
    /// Validates each element of <paramref name="validatables"/> and throws a
    /// <see cref="ValidationException"/> if any element is invalid. Unlike
    /// <see cref="ValidateOrThrow{T}(IReadOnlySet{T}, string)"/>, wraps the result in a
    /// <see cref="Valid{T}"/> so that later code can require an already-validated collection
    /// through its signature alone.
    /// </summary>
    /// <returns>A <see cref="Valid{T}"/> wrapping <paramref name="validatables"/>.</returns>
    public static Valid<IReadOnlySet<T>> ToValidOrThrow<T>(
        this IReadOnlySet<T> validatables,
        [CallerArgumentExpression(nameof(validatables))] string? basePath = null)
        where T : IValidatable
    {
        var errors = new ErrorCollector();
        errors.Nested(validatables, basePath!);
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return Valid<IReadOnlySet<T>>.Create(validatables);
    }
    
    /// <summary>
    /// Validates each value in <paramref name="validatables"/> and returns the collected errors, if any.
    /// </summary>
    public static ErrorCollector Validate<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> validatables, ErrorCollector? errors = null)
        where TValue : IValidatable
        where TKey : notnull
    {
        errors ??= new ErrorCollector();
        errors.Nested(validatables, "");
        return errors;
    }

    /// <summary>
    /// Validates each value in <paramref name="validatables"/> and throws a <see cref="ValidationException"/>
    /// if any errors are found.
    /// </summary>
    /// <returns><paramref name="validatables"/>, if validation succeeds.</returns>
    public static IReadOnlyDictionary<TKey, TValue> ValidateOrThrow<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> validatables, ErrorCollector? errors = null)
        where TValue : IValidatable
        where TKey : notnull
    {
        errors ??= new ErrorCollector();
        errors.Nested(validatables, "");
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return validatables;
    }
    
    /// <summary>
    /// Validates each item in <paramref name="validatables"/> and returns the collected errors, if any.
    /// </summary>
    public static ErrorCollector Validate<T>(this IEnumerable<T> validatables, ErrorCollector? errors = null)
        where T : IValidatable
    {
        var materialized = validatables.ToList();
        errors ??= new ErrorCollector();
        errors.Nested(materialized, "");
        return errors;
    }

    /// <summary>
    /// Validates each item in <paramref name="validatables"/> and throws a <see cref="ValidationException"/>
    /// if any errors are found.
    /// </summary>
    /// <returns><paramref name="validatables"/>, if validation succeeds.</returns>
    public static IEnumerable<T> ValidateOrThrow<T>(this IEnumerable<T> validatables, ErrorCollector? errors = null)
        where T : IValidatable
    {
        var materialized = validatables.ToList();
        errors ??= new ErrorCollector();
        errors.Nested(materialized, "");
        if (errors.Any())
            throw new ValidationException(errors.ToList());
        return materialized;
    }
}