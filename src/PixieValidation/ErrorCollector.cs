using System.Collections;
using System.Runtime.CompilerServices;

namespace PixieValidation;

public sealed class ErrorCollector : IEnumerable<ValidationError>
{
    private readonly List<ValidationError> _errors = [];
    private readonly Stack<string> _path = new();

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
        [CallerArgumentExpression(nameof(value))] string? propertyName = null)
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
        IReadOnlyCollection<T> values,
        PropChecker<T> checker,
        [CallerArgumentExpression(nameof(values))] string? basePath = null)
    {
        foreach (var error in CollectionValidationHelper.CollectErrors(values, checker, BuildPath(basePath!)))
            _errors.Add(error);
    }

    /// <summary>
    /// Validates <paramref name="child"/>, prefixing its errors' paths with <paramref name="propertyName"/>.
    /// </summary>
    public void Nested(
        IValidatable child,
        [CallerArgumentExpression(nameof(child))] string? propertyName = null)
    {
        _path.Push(propertyName!);
        child.ValidateAndCollect(this);
        _path.Pop();
    }
    
    /// <summary>
    /// Validates <paramref name="child"/> using <paramref name="validator"/>, prefixing its
    /// errors' paths with <paramref name="propertyName"/>.
    /// </summary>
    public void Nested<T>(
        IValidator<T> validator,
        T child,
        [CallerArgumentExpression(nameof(child))] string? propertyName = null)
    {
        _path.Push(propertyName!);
        validator.ValidateAndCollect(child, this);
        _path.Pop();
    }

    /// <summary>
    /// Validates each item in <paramref name="children"/>, prefixing errors' paths with
    /// <paramref name="propertyName"/> and the item's index (e.g. <c>Persons[1].Name</c>).
    /// </summary>
    public void Nested<T>(
        IReadOnlyList<T> children,
        [CallerArgumentExpression(nameof(children))] string? propertyName = null)
        where T : IValidatable
    {
        for (var i = 0; i < children.Count; i++)
        {
            _path.Push($"{propertyName}[{i}]");
            children[i].ValidateAndCollect(this);
            _path.Pop();
        }
    }
    
    /// <summary>
    /// Validates each item in <paramref name="children"/> using <paramref name="validator"/>,
    /// prefixing errors' paths with <paramref name="propertyName"/> and the item's index
    /// (e.g. <c>Persons[1].Name</c>).
    /// </summary>
    public void Nested<T>(
        IValidator<T> validator,
        IReadOnlyList<T> children,
        [CallerArgumentExpression(nameof(children))] string? propertyName = null)
    {
        for (var i = 0; i < children.Count; i++)
        {
            _path.Push($"{propertyName}[{i}]");
            validator.ValidateAndCollect(children[i], this);
            _path.Pop();
        }
    }
    
    /// <summary>
    /// Validates each item in <paramref name="children"/>, prefixing errors' paths with
    /// <paramref name="propertyName"/> and the item's iteration index (e.g. <c>Members[1].Name</c>).
    /// Since sets have no guaranteed order, this index may not be stable across calls.
    /// </summary>
    public void Nested<T>(
        IReadOnlySet<T> children,
        [CallerArgumentExpression(nameof(children))] string? propertyName = null)
        where T : IValidatable
    {
        var i = 0;
        foreach (var child in children)
        {
            _path.Push($"{propertyName}[{i}]");
            child.ValidateAndCollect(this);
            _path.Pop();
            i++;
        }
    }
    
    /// <summary>
    /// Validates each item in <paramref name="children"/> using <paramref name="validator"/>,
    /// prefixing errors' paths with <paramref name="propertyName"/> and the item's iteration index
    /// (e.g. <c>Members[1].Name</c>). Since sets have no guaranteed order, this index may not be
    /// stable across calls.
    /// </summary>
    public void Nested<T>(
        IValidator<T> validator,
        IReadOnlySet<T> children,
        [CallerArgumentExpression(nameof(children))] string? propertyName = null)
    {
        var i = 0;
        foreach (var child in children)
        {
            _path.Push($"{propertyName}[{i}]");
            validator.ValidateAndCollect(child, this);
            _path.Pop();
            i++;
        }
    }

    private string BuildPath(string propertyName) =>
        _path.Count == 0
            ? propertyName
            : $"{string.Join(".", _path.Reverse())}.{propertyName}";

    public IEnumerator<ValidationError> GetEnumerator() => _errors.GetEnumerator();
    
    // Explicit implementation of the non-generic IEnumerable, required because
    // IEnumerable<T> inherits from it. Not visible via IntelliSense on ErrorCollector directly;
    // only reachable when the instance is used as a plain IEnumerable. Forwards to the
    // public, generic GetEnumerator() above.
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}