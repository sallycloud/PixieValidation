namespace PixieValidation.PropCheckers;

public static class TimeSpanCheckers
{
    public static PropChecker<TimeSpan> Min(TimeSpan min) =>
        value => value < min ? $"Must be at least {min}." : null;

    public static PropChecker<TimeSpan> Max(TimeSpan max) =>
        value => value > max ? $"Must not exceed {max}." : null;

    public static PropChecker<TimeSpan> Range(TimeSpan min, TimeSpan max) =>
        value => value < min || value > max ? $"Must be between {min} and {max}." : null;

    public static PropChecker<TimeSpan> Positive() =>
        value => value <= TimeSpan.Zero ? "Must be positive." : null;

    public static PropChecker<TimeSpan> NonNegative() =>
        value => value < TimeSpan.Zero ? "Must not be negative." : null;
}