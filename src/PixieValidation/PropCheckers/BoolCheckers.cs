namespace PixieValidation.PropCheckers;

public static class BoolCheckers
{
    public static PropChecker<bool> IsTrue(string errorMessage = "Must be true.") =>
        value => !value ? errorMessage : null;

    public static PropChecker<bool> IsFalse(string errorMessage = "Must be false.") =>
        value => value ? errorMessage : null;
}