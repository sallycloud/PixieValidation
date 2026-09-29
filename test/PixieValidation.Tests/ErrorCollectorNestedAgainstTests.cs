using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorNestedAgainstTests
{
    [Fact]
    public void ValidateOrThrow_WithItemAtCapacity_DoesNotThrow()
    {
        var box = new SingleItemBox
        {
            Capacity = 5,
            Item = new Item { Size = 5 }
        };

        var exception = Record.Exception(() => box.ValidateOrThrow());

        Assert.Null(exception);
    }

    [Fact]
    public void ValidateOrThrow_WithItemAboveCapacity_ThrowsWithPrefixedPath()
    {
        var box = new SingleItemBox
        {
            Capacity = 5,
            Item = new Item { Size = 6 }
        };

        var exception = Assert.Throws<ValidationException>(() => box.ValidateOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("Item.Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void NestedAgainst_WithNullChild_AddsNullError()
    {
        var errors = new ErrorCollector();
        IValidatableAgainst<int>? item = null;

        errors.NestedAgainst(item, 5);

        var error = Assert.Single(errors);
        Assert.Equal("item", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    [Fact]
    public void NestedAgainst_PopsPathAfterValidation_SubsequentChecksUseOriginalPath()
    {
        var errors = new ErrorCollector();
        var item = new Item { Size = 6 };

        errors.NestedAgainst(item, 5);
        errors.Check("Other", "Some error.");

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Equal("item.Size", list[0].Path);
        Assert.Equal("Other", list[1].Path);
    }
    
    [Fact]
    public void Check_WithValidValueAgainstContext_DoesNotAddError()
    {
        var errors = new ErrorCollector();
        PropAgainstChecker<int, int> lessThanChecker = (value, against) => value < against ? null : "Must be less than context.";

        errors.CheckAgainst(3, 5, lessThanChecker);

        Assert.Empty(errors);
    }

    [Fact]
    public void Check_WithInvalidValueAgainstContext_AddsErrorWithInferredPropertyName()
    {
        var errors = new ErrorCollector();
        PropAgainstChecker<int, int> lessThanChecker = (value, against) => value < against ? null : "Must be less than context.";
        var quantity = 10;

        errors.CheckAgainst(quantity, 5, lessThanChecker);

        var error = Assert.Single(errors);
        Assert.Equal("quantity", error.Path);
        Assert.Equal("Must be less than context.", error.Message);
    }

    [Fact]
    public void Check_WithExplicitPropertyNameAgainstContext_UsesGivenName()
    {
        var errors = new ErrorCollector();
        PropAgainstChecker<int, int> lessThanChecker = (value, against) => value < against ? null : "Must be less than context.";

        errors.CheckAgainst(10, 5, lessThanChecker, "CustomName");

        var error = Assert.Single(errors);
        Assert.Equal("CustomName", error.Path);
    }
}