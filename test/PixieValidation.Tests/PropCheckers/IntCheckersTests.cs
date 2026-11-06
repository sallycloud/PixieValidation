using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class IntCheckersTests
{
    // Min

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(int.MaxValue)]
    public void Min_WithValueAtOrAboveMinimum_ReturnsNoError(int value)
    {
        var checker = IntCheckers.Min(5);

        Assert.Null(checker(value));
    }

    [Theory]
    [InlineData(4)]
    [InlineData(int.MinValue)]
    public void Min_WithValueBelowMinimum_ReturnsError(int value)
    {
        var checker = IntCheckers.Min(5);

        Assert.Equal("Must be at least 5.", checker(value));
    }

    // Max

    [Theory]
    [InlineData(5)]
    [InlineData(4)]
    [InlineData(int.MinValue)]
    public void Max_WithValueAtOrBelowMaximum_ReturnsNoError(int value)
    {
        var checker = IntCheckers.Max(5);

        Assert.Null(checker(value));
    }

    [Theory]
    [InlineData(6)]
    [InlineData(int.MaxValue)]
    public void Max_WithValueAboveMaximum_ReturnsError(int value)
    {
        var checker = IntCheckers.Max(5);

        Assert.Equal("Must not exceed 5.", checker(value));
    }

    // Range

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Range_WithValueInsideOrOnTheBounds_ReturnsNoError(int value)
    {
        var checker = IntCheckers.Range(1, 10);

        Assert.Null(checker(value));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void Range_WithValueOutsideTheBounds_ReturnsError(int value)
    {
        var checker = IntCheckers.Range(1, 10);

        Assert.Equal("Must be between 1 and 10.", checker(value));
    }

    // Positive

    [Theory]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void Positive_WithPositiveValue_ReturnsNoError(int value)
    {
        var checker = IntCheckers.Positive();

        Assert.Null(checker(value));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Positive_WithZeroOrNegativeValue_ReturnsError(int value)
    {
        var checker = IntCheckers.Positive();

        Assert.Equal("Must be positive.", checker(value));
    }

    // NonNegative

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void NonNegative_WithZeroOrPositiveValue_ReturnsNoError(int value)
    {
        var checker = IntCheckers.NonNegative();

        Assert.Null(checker(value));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void NonNegative_WithNegativeValue_ReturnsError(int value)
    {
        var checker = IntCheckers.NonNegative();

        Assert.Equal("Must not be negative.", checker(value));
    }
}
