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
        [CallerArgumentExpression(nameof(value))]
        string? propertyName = null)
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
        IReadOnlyCollection<T>? values,
        PropChecker<T> checker,
        [CallerArgumentExpression(nameof(values))]
        string? basePath = null)
    {
        foreach (var error in CollectionValidationHelper.CollectErrors(values, checker, BuildPath(basePath!)))
            _errors.Add(error);
    }

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

    /// <summary>
    /// Validates <paramref name="child"/> against <paramref name="against"/>, prefixing its errors'
    /// paths with <paramref name="propertyName"/>.
    /// </summary>
    public void NestedAgainst<TAgainst>(
        IValidatableAgainst<TAgainst>? child,
        TAgainst against,
        [CallerArgumentExpression(nameof(child))]
        string? propertyName = null)
    {
        if (CheckNotNull(child, propertyName)) return;

        _path.Push(propertyName!);
        child!.ValidateAndCollectAgainst(this, against);
        _path.Pop();
    }

    /// <summary>
    /// Validates each item in <paramref name="children"/> against <paramref name="against"/>,
    /// prefixing errors' paths with <paramref name="propertyName"/> and the item's enumeration
    /// index (e.g. <c>EbmPositions[1].Date</c>). Also covers lists and sets; for sets the index
    /// may not be stable across calls.
    /// </summary>
    public void NestedAgainst<T, TAgainst>(
        IEnumerable<T>? children,
        TAgainst against,
        [CallerArgumentExpression(nameof(children))]
        string? propertyName = null)
        where T : IValidatableAgainst<TAgainst>
    {
        if (CheckNotNull(children, propertyName)) return;

        var i = 0;
        foreach (var child in children!)
        {
            _path.Push($"{propertyName}[{i}]");
            child.ValidateAndCollectAgainst(this, against);
            _path.Pop();
            i++;
        }
    }

    /// <summary>
    /// Validates each value in <paramref name="children"/> against <paramref name="against"/>,
    /// prefixing errors' paths with <paramref name="propertyName"/> and the entry's key
    /// (e.g. <c>PositionsByRole[Coach].Date</c>).
    /// </summary>
    public void NestedAgainst<TKey, TValue, TAgainst>(
        IReadOnlyDictionary<TKey, TValue>? children,
        TAgainst against,
        [CallerArgumentExpression(nameof(children))]
        string? propertyName = null)
        where TValue : IValidatableAgainst<TAgainst>
        where TKey : notnull
    {
        if (CheckNotNull(children, propertyName)) return;

        foreach (var (key, value) in children!)
        {
            _path.Push($"{propertyName}[{key}]");
            value.ValidateAndCollectAgainst(this, against);
            _path.Pop();
        }
    }

    // Helpers

    private bool CheckNotNull(object? collection, string? propertyName)
    {
        if (collection is not null)
            return false;

        _errors.Add(new ValidationError(BuildPath(propertyName!), "Must not be null."));
        return true;
    }

    /// <summary>
    /// Validates <paramref name="child"/> using <paramref name="validator"/> against
    /// <paramref name="against"/>, prefixing its errors' paths with <paramref name="propertyName"/>.
    /// </summary>
    public void NestedAgainst<T, TAgainst>(
        IValidatorAgainst<T, TAgainst> validator,
        T? child,
        TAgainst against,
        [CallerArgumentExpression(nameof(child))]
        string? propertyName = null)
        where T : class
    {
        if (CheckNotNull(child, propertyName)) return;

        _path.Push(propertyName!);
        validator.ValidateAndCollectAgainst(child!, this, against);
        _path.Pop();
    }

    /// <summary>
    /// Validates each item in <paramref name="children"/> using <paramref name="validator"/>
    /// against <paramref name="against"/>, prefixing errors' paths with <paramref name="propertyName"/>
    /// and the item's enumeration index (e.g. <c>Items[1].Size</c>). Also covers lists and sets;
    /// for sets the index may not be stable across calls.
    /// </summary>
    public void NestedAgainst<T, TAgainst>(
        IValidatorAgainst<T, TAgainst> validator,
        IEnumerable<T>? children,
        TAgainst against,
        [CallerArgumentExpression(nameof(children))]
        string? propertyName = null)
    {
        if (CheckNotNull(children, propertyName)) return;

        var i = 0;
        foreach (var child in children!)
        {
            _path.Push($"{propertyName}[{i}]");
            validator.ValidateAndCollectAgainst(child, this, against);
            _path.Pop();
            i++;
        }
    }

    /// <summary>
    /// Validates each value in <paramref name="children"/> using <paramref name="validator"/>
    /// against <paramref name="against"/>, prefixing errors' paths with <paramref name="propertyName"/>
    /// and the entry's key (e.g. <c>ItemsBySlot[Left].Size</c>).
    /// </summary>
    public void NestedAgainst<TKey, TValue, TAgainst>(
        IValidatorAgainst<TValue, TAgainst> validator,
        IReadOnlyDictionary<TKey, TValue>? children,
        TAgainst against,
        [CallerArgumentExpression(nameof(children))]
        string? propertyName = null)
        where TKey : notnull
    {
        if (CheckNotNull(children, propertyName)) return;

        foreach (var (key, value) in children!)
        {
            _path.Push($"{propertyName}[{key}]");
            validator.ValidateAndCollectAgainst(value, this, against);
            _path.Pop();
        }
    }
    
    /// <summary>
    /// Runs <paramref name="checker"/> against <paramref name="value"/> and <paramref name="against"/>,
    /// and records the error, if any. The property name is inferred from the calling expression
    /// unless given explicitly.
    /// </summary>
    public void Check<T, TAgainst>(
        T value,
        TAgainst against,
        PropAgainstChecker<T, TAgainst> checker,
        [CallerArgumentExpression(nameof(value))]
        string? propertyName = null)
    {
        var error = checker(value, against);
        if (error is not null)
            _errors.Add(new ValidationError(BuildPath(propertyName!), error));
    }
}