namespace PixieValidation.PropCheckers;

public static class UriCheckers
{
    public static PropChecker<string> IsAbsoluteUri() =>
        value => Uri.TryCreate(value, UriKind.Absolute, out _)
            ? null
            : "Must be a valid absolute URI.";
    
    public static PropChecker<string> HasScheme(string scheme) =>
        value => Uri.TryCreate(value, UriKind.Absolute, out var uri) && uri.Scheme == scheme
            ? null
            : $"Must use the '{scheme}' scheme.";
    
    public static readonly PropChecker<string> IsHttp =
        HasScheme(Uri.UriSchemeHttp);
    
    public static readonly PropChecker<string> IsHttps =
        HasScheme(Uri.UriSchemeHttps);
    
    public static readonly PropChecker<string> IsHttpOrHttps =
        IsHttp.Or(IsHttps).WithMessage("Must use the 'http' or 'https' scheme.");
    
    public static readonly PropChecker<string> IsAbsoluteHttpUri =
        IsAbsoluteUri().And(IsHttpOrHttps);
    
    /// <summary>
    /// Fails if the raw string ends with a slash. Checks the string, not the parsed
    /// <see cref="Uri"/>: a parsed URI without a path always reports '/' as its path.
    /// </summary>
    public static readonly PropChecker<string> HasNoTrailingSlash =
        value => value.EndsWith('/') ? "Must not end with a trailing slash." : null;
}