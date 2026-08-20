using System.Collections;
using System.Runtime.CompilerServices;

namespace PixieValidation;

public sealed class ErrorCollector : IEnumerable<ValidationError>
{
    private readonly List<ValidationError> _errors = [];
    private readonly Stack<string> _path = new();

    public void Check(string propertyName, string? error)
    {
        if (error is not null)
            _errors.Add(new ValidationError(BuildPath(propertyName), error));
    }

    public void Check<T>(
        T value,
        Validator<T> validate,
        [CallerArgumentExpression(nameof(value))] string? propertyName = null)
    {
        var error = validate(value);
        if (error is not null)
            _errors.Add(new ValidationError(BuildPath(propertyName!), error));
    }

    public void Nested(
        IValidatable child,
        [CallerArgumentExpression(nameof(child))] string? propertyName = null)
    {
        _path.Push(propertyName!);
        child.Validate(this);
        _path.Pop();
    }

    public void Nested<T>(
        IReadOnlyList<T> children,
        [CallerArgumentExpression(nameof(children))] string? propertyName = null)
        where T : IValidatable
    {
        for (var i = 0; i < children.Count; i++)
        {
            _path.Push($"{propertyName}[{i}]");
            children[i].Validate(this);
            _path.Pop();
        }
    }

    private string BuildPath(string propertyName) =>
        _path.Count == 0
            ? propertyName
            : $"{string.Join(".", _path.Reverse())}.{propertyName}";

    public IEnumerator<ValidationError> GetEnumerator() => _errors.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}