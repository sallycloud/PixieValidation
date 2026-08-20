namespace PixieValidation.PropCheckers;

public static class UriCheckers
{
    public static PropChecker<string> IsAbsoluteUri() =>
        value => !Uri.TryCreate(value, UriKind.Absolute, out _) ? "Must be a valid absolute URI." : null;

    public static PropChecker<string> HasScheme(string scheme) =>
        value => !(Uri.TryCreate(value, UriKind.Absolute, out var uri) && uri.Scheme == scheme)
            ? $"Must use the '{scheme}' scheme."
            : null;
}