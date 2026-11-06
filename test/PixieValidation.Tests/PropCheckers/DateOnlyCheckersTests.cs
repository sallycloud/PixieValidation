using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class DateOnlyCheckersTests
{
    private static readonly DateOnly Past = DateOnly.FromDateTime(DateTime.Now).AddYears(-1);
    private static readonly DateOnly Future = DateOnly.FromDateTime(DateTime.Now).AddYears(1);

    [Fact]
    public void NotInPast_WithFutureValue_ReturnsNoError() => Assert.Null(DateOnlyCheckers.NotInPast()(Future));

    [Fact]
    public void NotInPast_WithPastValue_ReturnsError() =>
        Assert.Equal("Must not be in the past.", DateOnlyCheckers.NotInPast()(Past));

    [Fact]
    public void NotInFuture_WithPastValue_ReturnsNoError() => Assert.Null(DateOnlyCheckers.NotInFuture()(Past));

    [Fact]
    public void NotInFuture_WithFutureValue_ReturnsError() =>
        Assert.Equal("Must not be in the future.", DateOnlyCheckers.NotInFuture()(Future));

    [Fact]
    public void Range_InsideOrOnTheBounds_ReturnsNoError()
    {
        var min = new DateOnly(2020, 1, 1);
        var max = new DateOnly(2030, 1, 1);
        var checker = DateOnlyCheckers.Range(min, max);

        Assert.Null(checker(min));
        Assert.Null(checker(new DateOnly(2025, 6, 15)));
        Assert.Null(checker(max));
    }

    [Fact]
    public void Range_OutsideTheBounds_ReturnsError()
    {
        var min = new DateOnly(2020, 1, 1);
        var max = new DateOnly(2030, 1, 1);
        var checker = DateOnlyCheckers.Range(min, max);

        Assert.StartsWith("Must be between ", checker(min.AddDays(-1)));
        Assert.StartsWith("Must be between ", checker(max.AddDays(1)));
    }
}
