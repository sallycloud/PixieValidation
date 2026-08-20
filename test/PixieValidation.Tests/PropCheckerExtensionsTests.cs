namespace PixieValidation.Tests;

public class PropCheckerExtensionsTests
{
    private static readonly PropChecker<string> NotEmptyChecker = value =>
        string.IsNullOrEmpty(value) ? "Value is required." : null;

    [Fact]
    public void ValidateOrThrow_WithValidValue_ReturnsValue()
    {
        var result = NotEmptyChecker.ValidateOrThrow("Alice");

        Assert.Equal("Alice", result);
    }

    [Fact]
    public void ValidateOrThrow_WithInvalidValue_ThrowsWithPathAndMessage()
    {
        var name = "";

        var exception = Assert.Throws<ValidationException>(() => NotEmptyChecker.ValidateOrThrow(name));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ToValidOrThrow_WithValidValue_ReturnsValidWrappingValue()
    {
        Valid<string> result = NotEmptyChecker.ToValidOrThrow("Alice");

        Assert.Equal("Alice", result.Value);
    }

    [Fact]
    public void ToValidOrThrow_WithInvalidValue_ThrowsWithPathAndMessage()
    {
        var name = "";

        var exception = Assert.Throws<ValidationException>(() => { NotEmptyChecker.ToValidOrThrow(name); });

        var error = Assert.Single(exception.Errors);
        Assert.Equal("name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }
}