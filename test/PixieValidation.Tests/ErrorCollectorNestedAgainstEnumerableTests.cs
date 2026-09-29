using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorNestedAgainstEnumerableTests
{
    [Fact]
    public void ValidateOrThrow_WithAllItemsWithinCapacity_DoesNotThrow()
    {
        var box = new MultiItemBox
        {
            Capacity = 5,
            Items = [new Item { Size = 3 }, new Item { Size = 5 }]
        };

        var exception = Record.Exception(() => box.ValidateOrThrow());

        Assert.Null(exception);
    }

    [Fact]
    public void ValidateOrThrow_WithOneItemAboveCapacity_ThrowsWithIndexedPath()
    {
        var box = new MultiItemBox
        {
            Capacity = 5,
            Items = [new Item { Size = 3 }, new Item { Size = 6 }]
        };

        var exception = Assert.Throws<ValidationException>(() => box.ValidateOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("Items[1].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithMultipleItemsAboveCapacity_ThrowsWithAllIndexedPaths()
    {
        var box = new MultiItemBox
        {
            Capacity = 5,
            Items = [new Item { Size = 7 }, new Item { Size = 2 }, new Item { Size = 9 }]
        };

        var exception = Assert.Throws<ValidationException>(() => box.ValidateOrThrow());

        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "Items[0].Size" && e.Message == "Must not exceed 5.");
        Assert.Contains(exception.Errors, e => e.Path == "Items[2].Size" && e.Message == "Must not exceed 5.");
    }

    [Fact]
    public void ValidateOrThrow_WithEmptyItems_DoesNotThrow()
    {
        var box = new MultiItemBox
        {
            Capacity = 5,
            Items = []
        };

        var exception = Record.Exception(() => box.ValidateOrThrow());

        Assert.Null(exception);
    }

    [Fact]
    public void NestedAgainst_WithNullEnumerable_AddsNullError()
    {
        var errors = new ErrorCollector();
        IEnumerable<Item>? items = null;

        errors.NestedAgainst(items, 5);

        var error = Assert.Single(errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    [Fact]
    public void NestedAgainst_WithSetOfChildren_AddsOneErrorPerInvalidChild()
    {
        var errors = new ErrorCollector();
        IReadOnlySet<Item> items = new HashSet<Item>
        {
            new() { Size = 3 },
            new() { Size = 6 }
        };

        errors.NestedAgainst(items, 5);

        var error = Assert.Single(errors);
        Assert.StartsWith("items[", error.Path);
        Assert.EndsWith("].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }
}