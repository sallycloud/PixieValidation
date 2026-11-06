using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class DateTimeCheckersTests
{
    private static readonly DateTime Past = DateTime.Now.AddYears(-1);
    private static readonly DateTime Future = DateTime.Now.AddYears(1);

    [Fact]
    public void NotInPast_WithFutureValue_ReturnsNoError() => Assert.Null(DateTimeCheckers.NotInPast()(Future));

    [Fact]
    public void NotInPast_WithPastValue_ReturnsError() =>
        Assert.Equal("Must not be in the past.", DateTimeCheckers.NotInPast()(Past));

    [Fact]
    public void NotInFuture_WithPastValue_ReturnsNoError() => Assert.Null(DateTimeCheckers.NotInFuture()(Past));

    [Fact]
    public void NotInFuture_WithFutureValue_ReturnsError() =>
        Assert.Equal("Must not be in the future.", DateTimeCheckers.NotInFuture()(Future));

    [Fact]
    public void Range_InsideOrOnTheBounds_ReturnsNoError()
    {
        var min = new DateTime(2020, 1, 1);
        var max = new DateTime(2030, 1, 1);
        var checker = DateTimeCheckers.Range(min, max);

        Assert.Null(checker(min));
        Assert.Null(checker(new DateTime(2025, 6, 15)));
        Assert.Null(checker(max));
    }

    [Fact]
    public void Range_OutsideTheBounds_ReturnsError()
    {
        var min = new DateTime(2020, 1, 1);
        var max = new DateTime(2030, 1, 1);
        var checker = DateTimeCheckers.Range(min, max);

        Assert.StartsWith("Must be between ", checker(min.AddTicks(-1)));
        Assert.StartsWith("Must be between ", checker(max.AddTicks(1)));
    }
}
