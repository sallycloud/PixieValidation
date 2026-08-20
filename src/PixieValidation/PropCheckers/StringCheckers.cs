using System.Text.RegularExpressions;

namespace PixieValidation.PropCheckers;

public static class StringCheckers
{
    public static PropChecker<string> NotEmpty() =>
        value => string.IsNullOrEmpty(value) ? "Value is required." : null;

    public static PropChecker<string> MinLength(int min) =>
        value => value.Length < min ? $"Must have at least {min} characters." : null;

    public static PropChecker<string> MaxLength(int max) =>
        value => value.Length > max ? $"Must not exceed {max} characters." : null;

    public static PropChecker<string> Matches(Regex pattern, string errorMessage = "Is malformed.") =>
        value => !pattern.IsMatch(value) ? errorMessage : null;
}