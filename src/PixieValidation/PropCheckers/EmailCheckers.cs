using System.Text.RegularExpressions;

namespace PixieValidation.PropCheckers;

public static class EmailCheckers
{
    private static readonly Regex Pattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public static PropChecker<string> IsValid() =>
        value => !Pattern.IsMatch(value) ? "Must be a valid email address." : null;
}