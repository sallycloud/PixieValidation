namespace PixieValidation.PropCheckers;

public static class DateTimeOffsetCheckers
{
    public static PropChecker<DateTimeOffset> NotInPast() =>
        value => value < DateTimeOffset.UtcNow ? "Must not be in the past." : null;

    public static PropChecker<DateTimeOffset> NotInFuture() =>
        value => value > DateTimeOffset.UtcNow ? "Must not be in the future." : null;

    public static PropChecker<DateTimeOffset> Range(DateTimeOffset min, DateTimeOffset max) =>
        value => value < min || value > max ? $"Must be between {min} and {max}." : null;
}