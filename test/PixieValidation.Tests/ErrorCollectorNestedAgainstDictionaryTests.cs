using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorNestedAgainstDictionaryTests
{
    [Fact]
    public void ValidateOrThrow_WithAllItemsWithinCapacity_DoesNotThrow()
    {
        var box = new SlottedBox
        {
            Capacity = 5,
            ItemsBySlot = new Dictionary<string, Item>
            {
                ["Left"] = new Item { Size = 3 },
                ["Right"] = new Item { Size = 5 }
            }
        };

        var exception = Record.Exception(() => box.ValidateOrThrow());

        Assert.Null(exception);
    }

    [Fact]
    public void ValidateOrThrow_WithOneItemAboveCapacity_ThrowsWithKeyedPath()
    {
        var box = new SlottedBox
        {
            Capacity = 5,
            ItemsBySlot = new Dictionary<string, Item>
            {
                ["Left"] = new Item { Size = 6 },
                ["Right"] = new Item { Size = 3 }
            }
        };

        var exception = Assert.Throws<ValidationException>(() => box.ValidateOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("ItemsBySlot[Left].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithMultipleItemsAboveCapacity_ThrowsWithAllKeyedPaths()
    {
        var box = new SlottedBox
        {
            Capacity = 5,
            ItemsBySlot = new Dictionary<string, Item>
            {
                ["Left"] = new Item { Size = 7 },
                ["Middle"] = new Item { Size = 2 },
                ["Right"] = new Item { Size = 9 }
            }
        };

        var exception = Assert.Throws<ValidationException>(() => box.ValidateOrThrow());

        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "ItemsBySlot[Left].Size" && e.Message == "Must not exceed 5.");
        Assert.Contains(exception.Errors, e => e.Path == "ItemsBySlot[Right].Size" && e.Message == "Must not exceed 5.");
    }

    [Fact]
    public void ValidateOrThrow_WithEmptyDictionary_DoesNotThrow()
    {
        var box = new SlottedBox
        {
            Capacity = 5,
            ItemsBySlot = new Dictionary<string, Item>()
        };

        var exception = Record.Exception(() => box.ValidateOrThrow());

        Assert.Null(exception);
    }

    [Fact]
    public void NestedAgainst_WithNullDictionary_AddsNullError()
    {
        var errors = new ErrorCollector();
        IReadOnlyDictionary<string, Item>? items = null;

        errors.NestedAgainst(items, 5);

        var error = Assert.Single(errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}