using System.Text.RegularExpressions;
using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class StringCheckersTests
{
    [Fact]
    public void NotEmpty_WithNonEmptyValue_ReturnsNoError()
    {
        var checker = StringCheckers.NotEmpty();

        var error = checker("Alice");

        Assert.Null(error);
    }

    [Fact]
    public void NotEmpty_WithEmptyValue_ReturnsDefaultMessage()
    {
        var checker = StringCheckers.NotEmpty();

        var error = checker("");

        Assert.Equal("Value is required.", error);
    }

    [Fact]
    public void NotEmpty_WithWhitespaceOnlyValue_ReturnsNoError()
    {
        // Unlike NotBlank, NotEmpty accepts whitespace-only values.
        var checker = StringCheckers.NotEmpty();

        var error = checker("   ");

        Assert.Null(error);
    }

    [Fact]
    public void NotEmpty_WithCustomMessage_ReturnsCustomMessage()
    {
        var checker = StringCheckers.NotEmpty("Name is required.");

        var error = checker("");

        Assert.Equal("Name is required.", error);
    }

    [Fact]
    public void NotBlank_WithNonEmptyValue_ReturnsNoError()
    {
        var checker = StringCheckers.NotBlank();

        var error = checker("Alice");

        Assert.Null(error);
    }

    [Fact]
    public void NotBlank_WithEmptyValue_ReturnsDefaultMessage()
    {
        var checker = StringCheckers.NotBlank();

        var error = checker("");

        Assert.Equal("Value is required.", error);
    }

    [Fact]
    public void NotBlank_WithWhitespaceOnlyValue_ReturnsDefaultMessage()
    {
        var checker = StringCheckers.NotBlank();

        var error = checker("   ");

        Assert.Equal("Value is required.", error);
    }

    [Fact]
    public void NotBlank_WithCustomMessage_ReturnsCustomMessage()
    {
        var checker = StringCheckers.NotBlank("client_id is required.");

        var error = checker(" ");

        Assert.Equal("client_id is required.", error);
    }

    [Fact]
    public void MinLength_WithLongEnoughValue_ReturnsNoError()
    {
        var checker = StringCheckers.MinLength(3);

        var error = checker("Alice");

        Assert.Null(error);
    }

    [Fact]
    public void MinLength_WithExactLength_ReturnsNoError()
    {
        var checker = StringCheckers.MinLength(5);

        var error = checker("Alice");

        Assert.Null(error);
    }

    [Fact]
    public void MinLength_WithTooShortValue_ReturnsError()
    {
        var checker = StringCheckers.MinLength(5);

        var error = checker("Al");

        Assert.Equal("Must have at least 5 characters.", error);
    }

    [Fact]
    public void MaxLength_WithShortEnoughValue_ReturnsNoError()
    {
        var checker = StringCheckers.MaxLength(10);

        var error = checker("Alice");

        Assert.Null(error);
    }

    [Fact]
    public void MaxLength_WithExactLength_ReturnsNoError()
    {
        var checker = StringCheckers.MaxLength(5);

        var error = checker("Alice");

        Assert.Null(error);
    }

    [Fact]
    public void MaxLength_WithTooLongValue_ReturnsError()
    {
        var checker = StringCheckers.MaxLength(3);

        var error = checker("Alice");

        Assert.Equal("Must not exceed 3 characters.", error);
    }

    [Fact]
    public void Matches_WithMatchingValue_ReturnsNoError()
    {
        var checker = StringCheckers.Matches(new Regex(@"^[a-z]+$"));

        var error = checker("alice");

        Assert.Null(error);
    }

    [Fact]
    public void Matches_WithNonMatchingValue_ReturnsDefaultMessage()
    {
        var checker = StringCheckers.Matches(new Regex(@"^[a-z]+$"));

        var error = checker("Alice1");

        Assert.Equal("Is malformed.", error);
    }

    [Fact]
    public void Matches_WithCustomMessage_ReturnsCustomMessage()
    {
        var checker = StringCheckers.Matches(new Regex(@"^[a-z]+$"), "Only lowercase letters allowed.");

        var error = checker("Alice1");

        Assert.Equal("Only lowercase letters allowed.", error);
    }

    [Fact]
    public void HasLowercase_WithLowercaseLetter_ReturnsNoError()
    {
        var checker = StringCheckers.HasLowercase();

        var error = checker("ALICe");

        Assert.Null(error);
    }

    [Fact]
    public void HasLowercase_WithoutLowercaseLetter_ReturnsDefaultMessage()
    {
        var checker = StringCheckers.HasLowercase();

        var error = checker("ALICE123");

        Assert.Equal("Must contain at least one lowercase letter.", error);
    }

    [Fact]
    public void HasLowercase_WithCustomMessage_ReturnsCustomMessage()
    {
        var checker = StringCheckers.HasLowercase("Needs a lowercase letter.");

        var error = checker("ALICE");

        Assert.Equal("Needs a lowercase letter.", error);
    }

    [Fact]
    public void HasUppercase_WithUppercaseLetter_ReturnsNoError()
    {
        var checker = StringCheckers.HasUppercase();

        var error = checker("aliCe");

        Assert.Null(error);
    }

    [Fact]
    public void HasUppercase_WithoutUppercaseLetter_ReturnsDefaultMessage()
    {
        var checker = StringCheckers.HasUppercase();

        var error = checker("alice123");

        Assert.Equal("Must contain at least one uppercase letter.", error);
    }

    [Fact]
    public void HasUppercase_WithCustomMessage_ReturnsCustomMessage()
    {
        var checker = StringCheckers.HasUppercase("Needs an uppercase letter.");

        var error = checker("alice");

        Assert.Equal("Needs an uppercase letter.", error);
    }

    [Fact]
    public void HasDigit_WithDigit_ReturnsNoError()
    {
        var checker = StringCheckers.HasDigit();

        var error = checker("Alice1");

        Assert.Null(error);
    }

    [Fact]
    public void HasDigit_WithoutDigit_ReturnsDefaultMessage()
    {
        var checker = StringCheckers.HasDigit();

        var error = checker("Alice");

        Assert.Equal("Must contain at least one digit.", error);
    }

    [Fact]
    public void HasDigit_WithCustomMessage_ReturnsCustomMessage()
    {
        var checker = StringCheckers.HasDigit("Needs a digit.");

        var error = checker("Alice");

        Assert.Equal("Needs a digit.", error);
    }

    [Fact]
    public void HasSpecialCharacter_WithAllowedCharacter_ReturnsNoError()
    {
        var checker = StringCheckers.HasSpecialCharacter("!@#");

        var error = checker("Alice!");

        Assert.Null(error);
    }

    [Fact]
    public void HasSpecialCharacter_WithoutAllowedCharacter_ReturnsDefaultMessage()
    {
        var checker = StringCheckers.HasSpecialCharacter("!@#");

        var error = checker("Alice123");

        Assert.Equal("Must contain at least one of the following characters: !@#", error);
    }

    [Fact]
    public void HasSpecialCharacter_WithCharacterNotInAllowedSet_ReturnsError()
    {
        // '^' is not in the allowed set, so it does not count as a special character here.
        var checker = StringCheckers.HasSpecialCharacter("!@#");

        var error = checker("Alice^");

        Assert.Equal("Must contain at least one of the following characters: !@#", error);
    }

    [Fact]
    public void HasSpecialCharacter_WithCustomMessage_ReturnsCustomMessage()
    {
        var checker = StringCheckers.HasSpecialCharacter("!@#", "Needs a special character.");

        var error = checker("Alice123");

        Assert.Equal("Needs a special character.", error);
    }
}