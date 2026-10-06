using System.Runtime.CompilerServices;

namespace PixieValidation;

public sealed partial class ErrorCollector
{
    /// <summary>
    /// Validates <paramref name="child"/>, prefixing its errors' paths with <paramref name="propertyName"/>.
    /// </summary>
    public void Nested(
        IValidatable? child,
        [CallerArgumentExpression(nameof(child))]
        string? propertyName = null)
    {
        if (CheckNotNull(child, propertyName)) return;

        _path.Push(propertyName!);
        child!.ValidateAndCollect(this);
        _path.Pop();
    }

    /// <summary>
    /// Validates <paramref name="child"/> using <paramref name="validator"/>, prefixing its
    /// errors' paths with <paramref name="propertyName"/>.
    /// </summary>
    public void Nested<T>(
        IValidator<T> validator,
        T? child,
        [CallerArgumentExpression(nameof(child))]
        string? propertyName = null)
        where T : class
    {
        if (CheckNotNull(child, propertyName)) return;

        _path.Push(propertyName!);
        validator.ValidateAndCollect(child!, this);
        _path.Pop();
    }

    /// <summary>
    /// Validates each value in <paramref name="children"/>, prefixing errors' paths with
    /// <paramref name="propertyName"/> and the entry's key (e.g. <c>PersonsByRole[Captain].Name</c>).
    /// </summary>
    public void Nested<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue>? children,
        [CallerArgumentExpression(nameof(children))]
        string? propertyName = null)
        where TValue : IValidatable
        where TKey : notnull
    {
        if (CheckNotNull(children, propertyName)) return;

        foreach (var (key, value) in children!)
        {
            _path.Push($"{propertyName}[{key}]");
            value.ValidateAndCollect(this);
            _path.Pop();
        }
    }

    /// <summary>
    /// Validates each value in <paramref name="children"/> using <paramref name="validator"/>,
    /// prefixing errors' paths with <paramref name="propertyName"/> and the entry's key
    /// (e.g. <c>PersonsByRole[Captain].Name</c>).
    /// </summary>
    public void Nested<TKey, TValue>(
        IValidator<TValue> validator,
        IReadOnlyDictionary<TKey, TValue>? children,
        [CallerArgumentExpression(nameof(children))]
        string? propertyName = null)
        where TKey : notnull
    {
        if (CheckNotNull(children, propertyName)) return;

        foreach (var (key, value) in children!)
        {
            _path.Push($"{propertyName}[{key}]");
            validator.ValidateAndCollect(value, this);
            _path.Pop();
        }
    }

    /// <summary>
    /// Validates each item in <paramref name="children"/>, prefixing errors' paths with
    /// <paramref name="propertyName"/> and the item's enumeration index (e.g. <c>Persons[1].Name</c>).
    /// </summary>
    public void Nested<T>(
        IEnumerable<T>? children,
        [CallerArgumentExpression(nameof(children))]
        string? propertyName = null)
        where T : IValidatable
    {
        if (CheckNotNull(children, propertyName)) return;

        var i = 0;
        foreach (var child in children!)
        {
            _path.Push($"{propertyName}[{i}]");
            child.ValidateAndCollect(this);
            _path.Pop();
            i++;
        }
    }

    /// <summary>
    /// Validates each item in <paramref name="children"/> using <paramref name="validator"/>,
    /// prefixing errors' paths with <paramref name="propertyName"/> and the item's enumeration index
    /// (e.g. <c>Persons[1].Name</c>).
    /// </summary>
    public void Nested<T>(
        IValidator<T> validator,
        IEnumerable<T>? children,
        [CallerArgumentExpression(nameof(children))]
        string? propertyName = null)
    {
        if (CheckNotNull(children, propertyName)) return;

        var i = 0;
        foreach (var child in children!)
        {
            _path.Push($"{propertyName}[{i}]");
            validator.ValidateAndCollect(child, this);
            _path.Pop();
            i++;
        }
    }
}
