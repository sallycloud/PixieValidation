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