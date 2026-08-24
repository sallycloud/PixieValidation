using System.Text.RegularExpressions;

namespace PixieValidation.PropCheckers;

public static class StringCheckers
{
    public static PropChecker<string> NotEmpty(string errorMessage = "Value is required.") =>
        value => string.IsNullOrEmpty(value) ? errorMessage : null;
    
    /// <summary>
    /// Checks that the value is neither <see langword="null"/>, empty, nor consists only of
    /// whitespace. Stricter than <see cref="NotEmpty"/>, which allows whitespace-only values.
    /// </summary>
    public static PropChecker<string> NotBlank(string errorMessage = "Value is required.") =>
        value => string.IsNullOrWhiteSpace(value) ? errorMessage : null;

    public static PropChecker<string> MinLength(int min) =>
        value => value.Length < min ? $"Must have at least {min} characters." : null;

    public static PropChecker<string> MaxLength(int max) =>
        value => value.Length > max ? $"Must not exceed {max} characters." : null;

    public static PropChecker<string> Matches(Regex pattern, string errorMessage = "Is malformed.") =>
        value => !pattern.IsMatch(value) ? errorMessage : null;
    
    public static PropChecker<string> HasLowercase(string errorMessage = "Must contain at least one lowercase letter.") =>
        value => value.Any(char.IsLower) ? null : errorMessage;

    public static PropChecker<string> HasUppercase(string errorMessage = "Must contain at least one uppercase letter.") =>
        value => value.Any(char.IsUpper) ? null : errorMessage;

    public static PropChecker<string> HasDigit(string errorMessage = "Must contain at least one digit.") =>
        value => value.Any(char.IsDigit) ? null : errorMessage;

    /// <summary>
    /// Checks that the value contains at least one character from <paramref name="allowedCharacters"/>.
    /// </summary>
    public static PropChecker<string> HasSpecialCharacter(string allowedCharacters, string? errorMessage = null) =>
        value => value.Any(allowedCharacters.Contains)
            ? null
            : errorMessage ?? $"Must contain at least one of the following characters: {allowedCharacters}";
}