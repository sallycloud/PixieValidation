namespace PixieValidation.Tests.PropCheckers;

public class NotCheckerTests
{
    private static readonly PropChecker<string> IsForbidden = value =>
        value == "admin" ? null : "Not forbidden.";

    [Fact]
    public void WithMessage_WhenWrappedCheckerFails_ReturnsNoError()
    {
        var checker = IsForbidden.Not().WithMessage("Invalid name.");

        var error = checker("alice");

        Assert.Null(error);
    }

    [Fact]
    public void WithMessage_WhenWrappedCheckerSucceeds_ReturnsGivenMessage()
    {
        var checker = IsForbidden.Not().WithMessage("Invalid name.");

        var error = checker("admin");

        Assert.Equal("Invalid name.", error);
    }
}