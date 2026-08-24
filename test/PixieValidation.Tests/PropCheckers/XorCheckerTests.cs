namespace PixieValidation.Tests.PropCheckers;

public class XorCheckerTests
{
    private static readonly PropChecker<int> IsEven = value => value % 2 == 0 ? null : "Must be even.";
    private static readonly PropChecker<int> IsNegative = value => value < 0 ? null : "Must be negative.";

    [Fact]
    public void WithMessage_WhenNeitherCheckerSucceeds_ReturnsGivenMessage()
    {
        var checker = IsEven.Xor(IsNegative).WithMessage("Must be even or negative, but not both.");

        var error = checker(3);

        Assert.Equal("Must be even or negative, but not both.", error);
    }

    [Fact]
    public void WithMessage_WhenOnlyFirstCheckerSucceeds_ReturnsNoError()
    {
        var checker = IsEven.Xor(IsNegative).WithMessage("Must be even or negative, but not both.");

        var error = checker(4);

        Assert.Null(error);
    }

    [Fact]
    public void WithMessage_WhenOnlySecondCheckerSucceeds_ReturnsNoError()
    {
        var checker = IsEven.Xor(IsNegative).WithMessage("Must be even or negative, but not both.");

        var error = checker(-3);

        Assert.Null(error);
    }

    [Fact]
    public void WithMessage_WhenBothCheckersSucceed_ReturnsGivenMessage()
    {
        var checker = IsEven.Xor(IsNegative).WithMessage("Must be even or negative, but not both.");

        var error = checker(-4);

        Assert.Equal("Must be even or negative, but not both.", error);
    }
}