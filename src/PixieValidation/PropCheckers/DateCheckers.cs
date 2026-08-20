namespace PixieValidation.PropCheckers;

public static class DateOnlyCheckers
{
    public static PropChecker<DateOnly> NotInPast() =>
        value => value < DateOnly.FromDateTime(DateTime.Now) ? "Must not be in the past." : null;

    public static PropChecker<DateOnly> NotInFuture() =>
        value => value > DateOnly.FromDateTime(DateTime.Now) ? "Must not be in the future." : null;

    public static PropChecker<DateOnly> Range(DateOnly min, DateOnly max) =>
        value => value < min || value > max ? $"Must be between {min} and {max}." : null;
}