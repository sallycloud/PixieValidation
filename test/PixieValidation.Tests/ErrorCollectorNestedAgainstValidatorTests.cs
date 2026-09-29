using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorNestedAgainstValidatorTests
{
    private static readonly ItemValidator Validator = new();

    [Fact]
    public void NestedAgainst_WithItemAtCapacity_DoesNotAddError()
    {
        var errors = new ErrorCollector();
        var item = new Item { Size = 5 };

        errors.NestedAgainst(Validator, item, 5);

        Assert.Empty(errors);
    }

    [Fact]
    public void NestedAgainst_WithItemAboveCapacity_AddsErrorWithPrefixedPath()
    {
        var errors = new ErrorCollector();
        var item = new Item { Size = 6 };

        errors.NestedAgainst(Validator, item, 5);

        var error = Assert.Single(errors);
        Assert.Equal("item.Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void NestedAgainst_WithNullChild_AddsNullError()
    {
        var errors = new ErrorCollector();
        Item? item = null;

        errors.NestedAgainst(Validator, item, 5);

        var error = Assert.Single(errors);
        Assert.Equal("item", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    [Fact]
    public void NestedAgainst_PopsPathAfterValidation_SubsequentChecksUseOriginalPath()
    {
        var errors = new ErrorCollector();
        var item = new Item { Size = 6 };

        errors.NestedAgainst(Validator, item, 5);
        errors.Check("Other", "Some error.");

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Equal("item.Size", list[0].Path);
        Assert.Equal("Other", list[1].Path);
    }
}