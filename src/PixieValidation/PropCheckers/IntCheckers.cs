namespace PixieValidation.PropCheckers;

public static class IntCheckers
{
    public static PropChecker<int> Min(int min) =>
        value => value < min ? $"Must be at least {min}." : null;

    public static PropChecker<int> Max(int max) =>
        value => value > max ? $"Must not exceed {max}." : null;

    public static PropChecker<int> Range(int min, int max) =>
        value => value < min || value > max ? $"Must be between {min} and {max}." : null;

    public static PropChecker<int> Positive() =>
        value => value <= 0 ? "Must be positive." : null;

    public static PropChecker<int> NonNegative() =>
        value => value < 0 ? "Must not be negative." : null;
}