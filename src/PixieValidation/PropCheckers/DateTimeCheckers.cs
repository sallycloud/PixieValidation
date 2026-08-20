namespace PixieValidation.PropCheckers;

public static class DateTimeCheckers
{
    public static PropChecker<DateTime> NotInPast() =>
        value => value < DateTime.Now ? "Must not be in the past." : null;

    public static PropChecker<DateTime> NotInFuture() =>
        value => value > DateTime.Now ? "Must not be in the future." : null;

    public static PropChecker<DateTime> Range(DateTime min, DateTime max) =>
        value => value < min || value > max ? $"Must be between {min} and {max}." : null;
}