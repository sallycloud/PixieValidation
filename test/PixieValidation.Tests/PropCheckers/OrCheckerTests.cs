namespace PixieValidation.Tests.PropCheckers;

public class OrCheckerTests
{
    private static readonly PropChecker<int> IsEven = value => value % 2 == 0 ? null : "Must be even.";
    private static readonly PropChecker<int> IsNegative = value => value < 0 ? null : "Must be negative.";
    private static readonly PropChecker<int> IsZero = value => value == 0 ? null : "Must be zero.";

    [Fact]
    public void WithMessage_WhenFirstCheckerSucceeds_ReturnsNoError()
    {
        var checker = IsEven.Or(IsNegative).WithMessage("Must be even or negative.");

        var error = checker(4);

        Assert.Null(error);
    }

    [Fact]
    public void WithMessage_WhenSecondCheckerSucceeds_ReturnsNoError()
    {
        var checker = IsEven.Or(IsNegative).WithMessage("Must be even or negative.");

        var error = checker(-3);

        Assert.Null(error);
    }

    [Fact]
    public void WithMessage_WhenBothCheckersSucceed_ReturnsNoError()
    {
        var checker = IsEven.Or(IsNegative).WithMessage("Must be even or negative.");

        var error = checker(-4);

        Assert.Null(error);
    }

    [Fact]
    public void WithMessage_WhenBothCheckersFail_ReturnsGivenMessage()
    {
        var checker = IsEven.Or(IsNegative).WithMessage("Must be even or negative.");

        var error = checker(3);

        Assert.Equal("Must be even or negative.", error);
    }

    [Fact]
    public void Or_ChainedWithThirdChecker_SucceedsIfAnyAlternativeSucceeds()
    {
        var checker = IsEven.Or(IsNegative).Or(IsZero).WithMessage("Must be even, negative, or zero.");

        var error = checker(0);

        Assert.Null(error);
    }

    [Fact]
    public void Or_ChainedWithThirdChecker_FailsWithGivenMessageWhenAllAlternativesFail()
    {
        var checker = IsEven.Or(IsNegative).Or(IsZero).WithMessage("Must be even, negative, or zero.");

        var error = checker(3);

        Assert.Equal("Must be even, negative, or zero.", error);
    }
}