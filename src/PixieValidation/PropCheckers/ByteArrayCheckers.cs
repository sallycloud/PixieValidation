namespace PixieValidation.PropCheckers;

public static class ByteArrayCheckers
{
    public static PropChecker<byte[]> NotEmpty() =>
        value => value.Length == 0 ? "Must not be empty." : null;

    public static PropChecker<byte[]> MinLength(int min) =>
        value => value.Length < min ? $"Must be at least {min} bytes." : null;

    public static PropChecker<byte[]> MaxLength(int max) =>
        value => value.Length > max ? $"Must not exceed {max} bytes." : null;
}