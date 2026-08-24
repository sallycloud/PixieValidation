namespace PixieValidation.PropCheckers;

public static class DoubleCheckers
{
    public static PropChecker<double> Min(double min) =>
        value => value < min ? $"Must be at least {min}." : null;

    public static PropChecker<double> Max(double max) =>
        value => value > max ? $"Must not exceed {max}." : null;

    public static PropChecker<double> Range(double min, double max) =>
        value => value < min || value > max ? $"Must be between {min} and {max}." : null;

    public static PropChecker<double> Positive() =>
        value => value <= 0 ? "Must be positive." : null;

    public static PropChecker<double> NonNegative() =>
        value => value < 0 ? "Must not be negative." : null;

    public static PropChecker<double> NotNaN() =>
        value => double.IsNaN(value) ? "Must not be NaN." : null;
}