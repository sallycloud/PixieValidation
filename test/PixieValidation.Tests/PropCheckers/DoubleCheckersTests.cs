using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class DoubleCheckersTests
{
    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public void Min_AtOrAboveMinimum_ReturnsNoError(int value) => Assert.Null(DoubleCheckers.Min(5)(value));

    [Theory]
    [InlineData(4)]
    [InlineData(-100)]
    public void Min_BelowMinimum_ReturnsError(int value) =>
        Assert.Equal("Must be at least 5.", DoubleCheckers.Min(5)(value));

    [Theory]
    [InlineData(5)]
    [InlineData(-100)]
    public void Max_AtOrBelowMaximum_ReturnsNoError(int value) => Assert.Null(DoubleCheckers.Max(5)(value));

    [Theory]
    [InlineData(6)]
    [InlineData(100)]
    public void Max_AboveMaximum_ReturnsError(int value) =>
        Assert.Equal("Must not exceed 5.", DoubleCheckers.Max(5)(value));

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Range_InsideOrOnTheBounds_ReturnsNoError(int value) =>
        Assert.Null(DoubleCheckers.Range(1, 10)(value));

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Range_OutsideTheBounds_ReturnsError(int value) =>
        Assert.Equal("Must be between 1 and 10.", DoubleCheckers.Range(1, 10)(value));

    [Fact]
    public void Positive_WithSmallPositiveValue_ReturnsNoError() =>
        Assert.Null(DoubleCheckers.Positive()(0.0001));

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Positive_WithZeroOrNegativeValue_ReturnsError(int value) =>
        Assert.Equal("Must be positive.", DoubleCheckers.Positive()(value));

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void NonNegative_WithZeroOrPositiveValue_ReturnsNoError(int value) =>
        Assert.Null(DoubleCheckers.NonNegative()(value));

    [Fact]
    public void NonNegative_WithSmallNegativeValue_ReturnsError() =>
        Assert.Equal("Must not be negative.", DoubleCheckers.NonNegative()(-0.0001));

    [Fact]
    public void NotNaN_WithNumber_ReturnsNoError() => Assert.Null(DoubleCheckers.NotNaN()(1.5));

    [Fact]
    public void NotNaN_WithNaN_ReturnsError() =>
        Assert.Equal("Must not be NaN.", DoubleCheckers.NotNaN()(double.NaN));
}
