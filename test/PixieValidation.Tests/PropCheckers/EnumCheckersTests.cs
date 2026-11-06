using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class EnumCheckersTests
{
    public enum Color
    {
        Red = 1,
        Green = 2
    }

    [Theory]
    [InlineData(Color.Red)]
    [InlineData(Color.Green)]
    public void IsDefined_WithDefinedValue_ReturnsNoError(Color value) =>
        Assert.Null(EnumCheckers.IsDefined<Color>()(value));

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    public void IsDefined_WithUndefinedValue_ReturnsError(int raw) =>
        Assert.Equal("Is not a valid value.", EnumCheckers.IsDefined<Color>()((Color)raw));
}
