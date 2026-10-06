using System.Runtime.CompilerServices;

namespace PixieValidation;

public sealed partial class ErrorCollector
{
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
}