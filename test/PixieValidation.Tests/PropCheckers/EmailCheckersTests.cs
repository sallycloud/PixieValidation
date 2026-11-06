using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class EmailCheckersTests
{
    private const string Message = "Must be a valid email address.";

    [Theory]
    [InlineData("john.doe@example.com")]
    [InlineData("a@b.c")]
    [InlineData("john+tag@example.co.uk")]
    public void IsValid_WithValidAddress_ReturnsNoError(string email)
    {
        var checker = EmailCheckers.IsValid();

        Assert.Null(checker(email));
    }

    [Theory]
    [InlineData("")]
    [InlineData("john.doe")]
    [InlineData("@example.com")]
    [InlineData("john@")]
    [InlineData("john@example")]
    [InlineData("john@example.")]
    [InlineData("john@.com")]
    [InlineData("john@@example.com")]
    public void IsValid_WithMalformedAddress_ReturnsError(string email)
    {
        var checker = EmailCheckers.IsValid();

        Assert.Equal(Message, checker(email));
    }

    [Theory]
    [InlineData("john doe@example.com")]
    [InlineData("john@exam ple.com")]
    [InlineData(" john@example.com")]
    [InlineData("john@example.com ")]
    public void IsValid_WithWhitespace_ReturnsError(string email)
    {
        var checker = EmailCheckers.IsValid();

        Assert.Equal(Message, checker(email));
    }

    [Fact]
    public void IsValid_WithTrailingNewline_ReturnsError()
    {
        var checker = EmailCheckers.IsValid();

        Assert.Equal(Message, checker("john@example.com\n"));
    }
}
