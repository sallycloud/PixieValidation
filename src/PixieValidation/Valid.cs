namespace PixieValidation;

public sealed class Valid<T>
{
    public T Value { get; }

    private Valid(T value) => Value = value;

    internal static Valid<T> Create(T value) => new(value);
}