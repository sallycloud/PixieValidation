using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class TimeSpanCheckersTests
{
    private static TimeSpan S(int seconds) => TimeSpan.FromSeconds(seconds);

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public void Min_AtOrAboveMinimum_ReturnsNoError(int seconds) =>
        Assert.Null(TimeSpanCheckers.Min(S(5))(S(seconds)));

    [Theory]
    [InlineData(4)]
    [InlineData(-10)]
    public void Min_BelowMinimum_ReturnsError(int seconds) =>
        Assert.Equal("Must be at least 00:00:05.", TimeSpanCheckers.Min(S(5))(S(seconds)));

    [Theory]
    [InlineData(5)]
    [InlineData(-10)]
    public void Max_AtOrBelowMaximum_ReturnsNoError(int seconds) =>
        Assert.Null(TimeSpanCheckers.Max(S(5))(S(seconds)));

    [Theory]
    [InlineData(6)]
    [InlineData(100)]
    public void Max_AboveMaximum_ReturnsError(int seconds) =>
        Assert.Equal("Must not exceed 00:00:05.", TimeSpanCheckers.Max(S(5))(S(seconds)));

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Range_InsideOrOnTheBounds_ReturnsNoError(int seconds) =>
        Assert.Null(TimeSpanCheckers.Range(S(1), S(10))(S(seconds)));

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Range_OutsideTheBounds_ReturnsError(int seconds) =>
        Assert.Equal("Must be between 00:00:01 and 00:00:10.", TimeSpanCheckers.Range(S(1), S(10))(S(seconds)));

    [Fact]
    public void Positive_WithPositiveValue_ReturnsNoError() =>
        Assert.Null(TimeSpanCheckers.Positive()(TimeSpan.FromTicks(1)));

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Positive_WithZeroOrNegativeValue_ReturnsError(int seconds) =>
        Assert.Equal("Must be positive.", TimeSpanCheckers.Positive()(S(seconds)));

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void NonNegative_WithZeroOrPositiveValue_ReturnsNoError(int seconds) =>
        Assert.Null(TimeSpanCheckers.NonNegative()(S(seconds)));

    [Fact]
    public void NonNegative_WithNegativeValue_ReturnsError() =>
        Assert.Equal("Must not be negative.", TimeSpanCheckers.NonNegative()(TimeSpan.FromTicks(-1)));
}
