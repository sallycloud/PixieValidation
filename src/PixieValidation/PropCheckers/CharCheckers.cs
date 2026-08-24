namespace PixieValidation.PropCheckers;

public static class CharCheckers
{
    public static PropChecker<char> IsDigit() =>
        value => !char.IsDigit(value) ? "Must be a digit." : null;

    public static PropChecker<char> IsLetter() =>
        value => !char.IsLetter(value) ? "Must be a letter." : null;

    public static PropChecker<char> IsLetterOrDigit() =>
        value => !char.IsLetterOrDigit(value) ? "Must be a letter or digit." : null;

    public static PropChecker<char> IsUpper() =>
        value => !char.IsUpper(value) ? "Must be uppercase." : null;

    public static PropChecker<char> IsLower() =>
        value => !char.IsLower(value) ? "Must be lowercase." : null;
}