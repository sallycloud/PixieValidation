namespace PixieValidation;

public sealed class ValidationException(IReadOnlyList<ValidationError> errors) : Exception
{
    public IReadOnlyList<ValidationError> Errors { get; } = errors;
}