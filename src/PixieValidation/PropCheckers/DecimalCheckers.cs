namespace PixieValidation.PropCheckers;

public static class DecimalCheckers
{
    public static PropChecker<decimal> Min(decimal min) =>
        value => value < min ? $"Must be at least {min}." : null;

    public static PropChecker<decimal> Max(decimal max) =>
        value => value > max ? $"Must not exceed {max}." : null;

    public static PropChecker<decimal> Range(decimal min, decimal max) =>
        value => value < min || value > max ? $"Must be between {min} and {max}." : null;

    public static PropChecker<decimal> Positive() =>
        value => value <= 0 ? "Must be positive." : null;

    public static PropChecker<decimal> NonNegative() =>
        value => value < 0 ? "Must not be negative." : null;
}