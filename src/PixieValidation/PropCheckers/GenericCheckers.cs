namespace PixieValidation.PropCheckers;

public static class GenericCheckers
{
    /// <summary>
    /// Checks that the value is contained in <paramref name="values"/>.
    /// </summary>
    public static PropChecker<T> OneOf<T>(IEnumerable<T> values, string? errorMessage = null) =>
        value => values.Contains(value) ? null : errorMessage ?? "Value is not one of the allowed values.";
}