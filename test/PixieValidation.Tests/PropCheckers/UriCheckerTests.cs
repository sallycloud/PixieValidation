using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class UriCheckersTests
{
    // IsAbsoluteUri

    [Theory]
    [InlineData("https://example.com")]
    [InlineData("http://example.com:8080/path")]
    [InlineData("mailto:alice@example.com")]
    public void IsAbsoluteUri_WithAbsoluteUri_ReturnsNoError(string value)
    {
        var checker = UriCheckers.IsAbsoluteUri();

        var error = checker(value);

        Assert.Null(error);
    }

    [Theory]
    [InlineData("example.com")]
    [InlineData("not a uri")]
    [InlineData("")]
    public void IsAbsoluteUri_WithoutScheme_ReturnsError(string value)
    {
        var checker = UriCheckers.IsAbsoluteUri();

        var error = checker(value);

        Assert.Equal("Must be a valid absolute URI.", error);
    }

    // HasScheme

    [Fact]
    public void HasScheme_WithMatchingScheme_ReturnsNoError()
    {
        var checker = UriCheckers.HasScheme("https");

        var error = checker("https://example.com");

        Assert.Null(error);
    }

    [Fact]
    public void HasScheme_WithUpperCaseInput_ReturnsNoError()
    {
        // Uri normalizes the scheme to lower case.
        var checker = UriCheckers.HasScheme("https");

        var error = checker("HTTPS://EXAMPLE.COM");

        Assert.Null(error);
    }

    [Fact]
    public void HasScheme_WithDifferentScheme_ReturnsError()
    {
        var checker = UriCheckers.HasScheme("https");

        var error = checker("http://example.com");

        Assert.Equal("Must use the 'https' scheme.", error);
    }

    [Fact]
    public void HasScheme_WithoutScheme_ReturnsSchemeError()
    {
        var checker = UriCheckers.HasScheme("https");

        var error = checker("example.com");

        Assert.Equal("Must use the 'https' scheme.", error);
    }

    // IsHttp

    [Fact]
    public void IsHttp_WithHttpUri_ReturnsNoError()
    {
        var checker = UriCheckers.IsHttp;

        var error = checker("http://example.com");

        Assert.Null(error);
    }

    [Fact]
    public void IsHttp_WithHttpsUri_ReturnsError()
    {
        var checker = UriCheckers.IsHttp;

        var error = checker("https://example.com");

        Assert.Equal("Must use the 'http' scheme.", error);
    }

    // IsHttps

    [Fact]
    public void IsHttps_WithHttpsUri_ReturnsNoError()
    {
        var checker = UriCheckers.IsHttps;

        var error = checker("https://example.com");

        Assert.Null(error);
    }

    [Fact]
    public void IsHttps_WithHttpUri_ReturnsError()
    {
        var checker = UriCheckers.IsHttps;

        var error = checker("http://example.com");

        Assert.Equal("Must use the 'https' scheme.", error);
    }

    // IsHttpOrHttps

    [Theory]
    [InlineData("http://example.com")]
    [InlineData("https://example.com")]
    public void IsHttpOrHttps_WithHttpOrHttpsUri_ReturnsNoError(string value)
    {
        var checker = UriCheckers.IsHttpOrHttps;

        var error = checker(value);

        Assert.Null(error);
    }

    [Theory]
    [InlineData("ftp://example.com")]
    [InlineData("mailto:alice@example.com")]
    public void IsHttpOrHttps_WithOtherScheme_ReturnsError(string value)
    {
        var checker = UriCheckers.IsHttpOrHttps;

        var error = checker(value);

        Assert.Equal("Must use the 'http' or 'https' scheme.", error);
    }

    // IsAbsoluteHttpUri

    [Theory]
    [InlineData("https://example.com")]
    [InlineData("http://example.com")]
    [InlineData("https://example.com:8443/auth")]
    public void IsAbsoluteHttpUri_WithHttpOrHttpsUri_ReturnsNoError(string value)
    {
        var checker = UriCheckers.IsAbsoluteHttpUri;

        var error = checker(value);

        Assert.Null(error);
    }

    [Fact]
    public void IsAbsoluteHttpUri_WithOtherScheme_ReturnsSchemeError()
    {
        var checker = UriCheckers.IsAbsoluteHttpUri;

        var error = checker("ftp://example.com");

        Assert.Equal("Must use the 'http' or 'https' scheme.", error);
    }

    [Theory]
    [InlineData("example.com")]
    [InlineData("")]
    public void IsAbsoluteHttpUri_WithoutScheme_ReturnsAbsoluteUriError(string value)
    {
        // And returns the first error, so the absolute-URI check wins over the scheme check.
        var checker = UriCheckers.IsAbsoluteHttpUri;

        var error = checker(value);

        Assert.Equal("Must be a valid absolute URI.", error);
    }

    [Fact]
    public void IsAbsoluteHttpUri_WithRelativePath_ReturnsError()
    {
        // On Unix, .NET may read "/foo" as an absolute file URI. Then the scheme check rejects it
        // instead of the absolute-URI check, so only the rejection itself is asserted.
        var checker = UriCheckers.IsAbsoluteHttpUri;

        var error = checker("/foo");

        Assert.NotNull(error);
    }

    // HasNoTrailingSlash

    [Theory]
    [InlineData("https://example.com")]
    [InlineData("https://example.com/auth")]
    public void HasNoTrailingSlash_WithoutTrailingSlash_ReturnsNoError(string value)
    {
        var checker = UriCheckers.HasNoTrailingSlash;

        var error = checker(value);

        Assert.Null(error);
    }

    [Theory]
    [InlineData("https://example.com/")]
    [InlineData("https://example.com/auth/")]
    [InlineData("/")]
    public void HasNoTrailingSlash_WithTrailingSlash_ReturnsError(string value)
    {
        var checker = UriCheckers.HasNoTrailingSlash;

        var error = checker(value);

        Assert.Equal("Must not end with a trailing slash.", error);
    }

    [Fact]
    public void HasNoTrailingSlash_WithEmptyString_ReturnsNoError()
    {
        // Emptiness is a different concern; combine with NotBlank where it matters.
        var checker = UriCheckers.HasNoTrailingSlash;

        var error = checker("");

        Assert.Null(error);
    }
}