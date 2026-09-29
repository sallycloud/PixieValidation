using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorNestedAgainstValidatorEnumerableTests
{
    private static readonly ItemValidator Validator = new();

    [Fact]
    public void NestedAgainst_WithAllItemsWithinCapacity_DoesNotAddErrors()
    {
        var errors = new ErrorCollector();
        var items = new[] { new Item { Size = 3 }, new Item { Size = 5 } };

        errors.NestedAgainst(Validator, items, 5);

        Assert.Empty(errors);
    }

    [Fact]
    public void NestedAgainst_WithOneItemAboveCapacity_AddsErrorWithIndexedPath()
    {
        var errors = new ErrorCollector();
        var items = new[] { new Item { Size = 3 }, new Item { Size = 6 } };

        errors.NestedAgainst(Validator, items, 5);

        var error = Assert.Single(errors);
        Assert.Equal("items[1].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void NestedAgainst_WithMultipleItemsAboveCapacity_AddsOneErrorPerInvalidItem()
    {
        var errors = new ErrorCollector();
        var items = new[] { new Item { Size = 7 }, new Item { Size = 2 }, new Item { Size = 9 } };

        errors.NestedAgainst(Validator, items, 5);

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, e => e.Path == "items[0].Size" && e.Message == "Must not exceed 5.");
        Assert.Contains(list, e => e.Path == "items[2].Size" && e.Message == "Must not exceed 5.");
    }

    [Fact]
    public void NestedAgainst_WithEmptyItems_DoesNotAddErrors()
    {
        var errors = new ErrorCollector();
        var items = Array.Empty<Item>();

        errors.NestedAgainst(Validator, items, 5);

        Assert.Empty(errors);
    }

    [Fact]
    public void NestedAgainst_WithNullEnumerable_AddsNullError()
    {
        var errors = new ErrorCollector();
        IEnumerable<Item>? items = null;

        errors.NestedAgainst(Validator, items, 5);

        var error = Assert.Single(errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    [Fact]
    public void NestedAgainst_WithSetOfItems_AddsOneErrorPerInvalidItem()
    {
        var errors = new ErrorCollector();
        IReadOnlySet<Item> items = new HashSet<Item> { new() { Size = 3 }, new() { Size = 6 } };

        errors.NestedAgainst(Validator, items, 5);

        var error = Assert.Single(errors);
        Assert.StartsWith("items[", error.Path);
        Assert.EndsWith("].Size", error.Path);
    }
}