using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class DateTimeOffsetCheckersTests
{
    private static readonly DateTimeOffset Past = DateTimeOffset.UtcNow.AddYears(-1);
    private static readonly DateTimeOffset Future = DateTimeOffset.UtcNow.AddYears(1);

    [Fact]
    public void NotInPast_WithFutureValue_ReturnsNoError() =>
        Assert.Null(DateTimeOffsetCheckers.NotInPast()(Future));

    [Fact]
    public void NotInPast_WithPastValue_ReturnsError() =>
        Assert.Equal("Must not be in the past.", DateTimeOffsetCheckers.NotInPast()(Past));

    [Fact]
    public void NotInFuture_WithPastValue_ReturnsNoError() =>
        Assert.Null(DateTimeOffsetCheckers.NotInFuture()(Past));

    [Fact]
    public void NotInFuture_WithFutureValue_ReturnsError() =>
        Assert.Equal("Must not be in the future.", DateTimeOffsetCheckers.NotInFuture()(Future));

    [Fact]
    public void Range_InsideOrOnTheBounds_ReturnsNoError()
    {
        var min = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var max = new DateTimeOffset(2030, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var checker = DateTimeOffsetCheckers.Range(min, max);

        Assert.Null(checker(min));
        Assert.Null(checker(new DateTimeOffset(2025, 6, 15, 0, 0, 0, TimeSpan.Zero)));
        Assert.Null(checker(max));
    }

    [Fact]
    public void Range_OutsideTheBounds_ReturnsError()
    {
        var min = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var max = new DateTimeOffset(2030, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var checker = DateTimeOffsetCheckers.Range(min, max);

        Assert.StartsWith("Must be between ", checker(min.AddTicks(-1)));
        Assert.StartsWith("Must be between ", checker(max.AddTicks(1)));
    }
}
