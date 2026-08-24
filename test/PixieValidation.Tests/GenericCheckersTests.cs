using PixieValidation.PropCheckers;

namespace PixieValidation.Tests;

public class GenericCheckersTests
{
    [Fact]
    public void OneOf_WithValueInSet_ReturnsNoError()
    {
        var checker = GenericCheckers.OneOf(["red", "green", "blue"]);

        var error = checker("green");

        Assert.Null(error);
    }

    [Fact]
    public void OneOf_WithValueNotInSet_ReturnsDefaultMessage()
    {
        var checker = GenericCheckers.OneOf(["red", "green", "blue"]);

        var error = checker("purple");

        Assert.Equal("Value is not one of the allowed values.", error);
    }

    [Fact]
    public void OneOf_WithValueNotInSet_AndCustomMessage_ReturnsCustomMessage()
    {
        var checker = GenericCheckers.OneOf(["red", "green", "blue"], "Not a valid color.");

        var error = checker("purple");

        Assert.Equal("Not a valid color.", error);
    }

    [Fact]
    public void OneOf_WithEmptySet_AlwaysReturnsError()
    {
        var checker = GenericCheckers.OneOf(Array.Empty<string>());

        var error = checker("anything");

        Assert.Equal("Value is not one of the allowed values.", error);
    }

    [Fact]
    public void OneOf_CombinedWithNot_RejectsValueInSet()
    {
        var checker = GenericCheckers.OneOf(["admin", "root"]).Not().WithMessage("Invalid name.");

        var error = checker("admin");

        Assert.Equal("Invalid name.", error);
    }

    [Fact]
    public void OneOf_CombinedWithNot_AcceptsValueNotInSet()
    {
        var checker = GenericCheckers.OneOf(["admin", "root"]).Not().WithMessage("Invalid name.");

        var error = checker("alice");

        Assert.Null(error);
    }
}