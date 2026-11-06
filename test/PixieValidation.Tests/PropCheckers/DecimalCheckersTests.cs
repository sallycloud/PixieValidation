using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class DecimalCheckersTests
{
    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public void Min_AtOrAboveMinimum_ReturnsNoError(int value) => Assert.Null(DecimalCheckers.Min(5)(value));

    [Theory]
    [InlineData(4)]
    [InlineData(-100)]
    public void Min_BelowMinimum_ReturnsError(int value) =>
        Assert.Equal("Must be at least 5.", DecimalCheckers.Min(5)(value));

    [Theory]
    [InlineData(5)]
    [InlineData(-100)]
    public void Max_AtOrBelowMaximum_ReturnsNoError(int value) => Assert.Null(DecimalCheckers.Max(5)(value));

    [Theory]
    [InlineData(6)]
    [InlineData(100)]
    public void Max_AboveMaximum_ReturnsError(int value) =>
        Assert.Equal("Must not exceed 5.", DecimalCheckers.Max(5)(value));

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Range_InsideOrOnTheBounds_ReturnsNoError(int value) =>
        Assert.Null(DecimalCheckers.Range(1, 10)(value));

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Range_OutsideTheBounds_ReturnsError(int value) =>
        Assert.Equal("Must be between 1 and 10.", DecimalCheckers.Range(1, 10)(value));

    [Fact]
    public void Positive_WithSmallPositiveValue_ReturnsNoError() =>
        Assert.Null(DecimalCheckers.Positive()(0.0001m));

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Positive_WithZeroOrNegativeValue_ReturnsError(int value) =>
        Assert.Equal("Must be positive.", DecimalCheckers.Positive()(value));

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void NonNegative_WithZeroOrPositiveValue_ReturnsNoError(int value) =>
        Assert.Null(DecimalCheckers.NonNegative()(value));

    [Fact]
    public void NonNegative_WithSmallNegativeValue_ReturnsError() =>
        Assert.Equal("Must not be negative.", DecimalCheckers.NonNegative()(-0.0001m));
}
