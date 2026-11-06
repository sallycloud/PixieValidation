using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class BoolCheckersTests
{
    [Fact]
    public void IsTrue_WithTrue_ReturnsNoError() => Assert.Null(BoolCheckers.IsTrue()(true));

    [Fact]
    public void IsTrue_WithFalse_ReturnsDefaultMessage() =>
        Assert.Equal("Must be true.", BoolCheckers.IsTrue()(false));

    [Fact]
    public void IsTrue_WithFalse_ReturnsCustomMessage() =>
        Assert.Equal("Please accept.", BoolCheckers.IsTrue("Please accept.")(false));

    [Fact]
    public void IsFalse_WithFalse_ReturnsNoError() => Assert.Null(BoolCheckers.IsFalse()(false));

    [Fact]
    public void IsFalse_WithTrue_ReturnsDefaultMessage() =>
        Assert.Equal("Must be false.", BoolCheckers.IsFalse()(true));

    [Fact]
    public void IsFalse_WithTrue_ReturnsCustomMessage() =>
        Assert.Equal("Must not be set.", BoolCheckers.IsFalse("Must not be set.")(true));
}
