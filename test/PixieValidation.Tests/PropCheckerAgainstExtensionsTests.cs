namespace PixieValidation.Tests;

public class PropAgainstCheckerExtensionsTests
{
    private static readonly PropAgainstChecker<string, int> NotEmptyChecker = (value, _) =>
        string.IsNullOrEmpty(value) ? "Value is required." : null;

    private static readonly PropAgainstChecker<string, int> MinLengthAgainstChecker = (value, against) =>
        value.Length < against ? "Too short for context." : null;

    [Fact]
    public void And_WithBothCheckersSucceeding_ReturnsNoError()
    {
        var checker = NotEmptyChecker.And(MinLengthAgainstChecker);

        var error = checker("Alice", 3);

        Assert.Null(error);
    }

    [Fact]
    public void And_WithFirstCheckerFailing_ReturnsFirstError()
    {
        var checker = NotEmptyChecker.And(MinLengthAgainstChecker);

        var error = checker("", 3);

        Assert.Equal("Value is required.", error);
    }

    [Fact]
    public void And_WithSecondCheckerFailing_ReturnsSecondError()
    {
        var checker = NotEmptyChecker.And(MinLengthAgainstChecker);

        var error = checker("Al", 3);

        Assert.Equal("Too short for context.", error);
    }
}