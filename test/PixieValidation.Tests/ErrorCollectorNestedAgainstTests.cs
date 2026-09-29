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
}