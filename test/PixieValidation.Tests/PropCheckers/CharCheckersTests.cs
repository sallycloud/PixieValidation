using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class CharCheckersTests
{
    [Theory]
    [InlineData('0')]
    [InlineData('5')]
    public void IsDigit_WithDigit_ReturnsNoError(char value) => Assert.Null(CharCheckers.IsDigit()(value));

    [Theory]
    [InlineData('a')]
    [InlineData('-')]
    public void IsDigit_WithOtherChar_ReturnsError(char value) =>
        Assert.Equal("Must be a digit.", CharCheckers.IsDigit()(value));

    [Theory]
    [InlineData('a')]
    [InlineData('Z')]
    [InlineData('ä')]
    public void IsLetter_WithLetter_ReturnsNoError(char value) => Assert.Null(CharCheckers.IsLetter()(value));

    [Theory]
    [InlineData('1')]
    [InlineData('-')]
    public void IsLetter_WithOtherChar_ReturnsError(char value) =>
        Assert.Equal("Must be a letter.", CharCheckers.IsLetter()(value));

    [Theory]
    [InlineData('a')]
    [InlineData('1')]
    public void IsLetterOrDigit_WithLetterOrDigit_ReturnsNoError(char value) =>
        Assert.Null(CharCheckers.IsLetterOrDigit()(value));

    [Theory]
    [InlineData('-')]
    [InlineData(' ')]
    public void IsLetterOrDigit_WithOtherChar_ReturnsError(char value) =>
        Assert.Equal("Must be a letter or digit.", CharCheckers.IsLetterOrDigit()(value));

    [Fact]
    public void IsUpper_WithUppercase_ReturnsNoError() => Assert.Null(CharCheckers.IsUpper()('A'));

    [Theory]
    [InlineData('a')]
    [InlineData('1')]
    public void IsUpper_WithOtherChar_ReturnsError(char value) =>
        Assert.Equal("Must be uppercase.", CharCheckers.IsUpper()(value));

    [Fact]
    public void IsLower_WithLowercase_ReturnsNoError() => Assert.Null(CharCheckers.IsLower()('a'));

    [Theory]
    [InlineData('A')]
    [InlineData('1')]
    public void IsLower_WithOtherChar_ReturnsError(char value) =>
        Assert.Equal("Must be lowercase.", CharCheckers.IsLower()(value));
}
