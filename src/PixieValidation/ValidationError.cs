namespace PixieValidation;

public sealed record ValidationError(string Path, string Message);