namespace PixieValidation;

/// <summary>
/// Wraps a value that has been proven valid according to its own <see cref="IValidatable"/>
/// implementation. This guarantee is intentionally narrow: only <c>ValidatableExtensions</c>
/// can construct a <see cref="Valid{T}"/>, and only by calling <typeparamref name="T"/>'s own
/// <see cref="IValidatable.ValidateAndCollect"/>. An external <see cref="IValidator{T}"/> or a
/// <see cref="PropChecker{T}"/> can validate a value, but can never produce a <see cref="Valid{T}"/> —
/// otherwise any caller could wrap any value by supplying a lax or empty validator.
/// </summary>
public sealed class Valid<T>
{
    public T Value { get; }

    private Valid(T value) => Value = value;

    internal static Valid<T> Create(T value) => new(value);
}