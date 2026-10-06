using System.Collections;

namespace PixieValidation;

public sealed partial class ErrorCollector : IEnumerable<ValidationError>
{
    private readonly List<ValidationError> _errors = [];
    
    private readonly Stack<string> _path = new();

    public IEnumerator<ValidationError> GetEnumerator() => _errors.GetEnumerator();

    // Explicit implementation of the non-generic IEnumerable, required because
    // IEnumerable<T> inherits from it. Not visible via IntelliSense on ErrorCollector directly;
    // only reachable when the instance is used as a plain IEnumerable. Forwards to the
    // public, generic GetEnumerator() above.
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Helpers

    private bool CheckNotNull(object? collection, string? propertyName)
    {
        if (collection is not null)
            return false;

        _errors.Add(new ValidationError(BuildPath(propertyName!), "Must not be null."));
        return true;
    }
    
    private string BuildPath(string propertyName)
    {
        if (_path.Count == 0)
            return propertyName;

        var prefix = string.Join(".", _path.Reverse());
        return string.IsNullOrEmpty(propertyName)
            ? prefix
            : $"{prefix}.{propertyName}";
    }
}
