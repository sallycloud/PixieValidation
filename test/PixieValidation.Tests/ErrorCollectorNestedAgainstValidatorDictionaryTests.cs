using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorNestedAgainstValidatorDictionaryTests
{
    private static readonly ItemValidator Validator = new();

    [Fact]
    public void NestedAgainst_WithAllItemsWithinCapacity_DoesNotAddErrors()
    {
        var errors = new ErrorCollector();
        var items = new Dictionary<string, Item>
        {
            ["Left"] = new Item { Size = 3 },
            ["Right"] = new Item { Size = 5 }
        };

        errors.NestedAgainst(Validator, items, 5);

        Assert.Empty(errors);
    }

    [Fact]
    public void NestedAgainst_WithOneItemAboveCapacity_AddsErrorWithKeyedPath()
    {
        var errors = new ErrorCollector();
        var items = new Dictionary<string, Item>
        {
            ["Left"] = new Item { Size = 6 },
            ["Right"] = new Item { Size = 3 }
        };

        errors.NestedAgainst(Validator, items, 5);

        var error = Assert.Single(errors);
        Assert.Equal("items[Left].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void NestedAgainst_WithMultipleItemsAboveCapacity_AddsOneErrorPerInvalidItem()
    {
        var errors = new ErrorCollector();
        var items = new Dictionary<string, Item>
        {
            ["Left"] = new Item { Size = 7 },
            ["Middle"] = new Item { Size = 2 },
            ["Right"] = new Item { Size = 9 }
        };

        errors.NestedAgainst(Validator, items, 5);

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, e => e.Path == "items[Left].Size" && e.Message == "Must not exceed 5.");
        Assert.Contains(list, e => e.Path == "items[Right].Size" && e.Message == "Must not exceed 5.");
    }

    [Fact]
    public void NestedAgainst_WithEmptyDictionary_DoesNotAddErrors()
    {
        var errors = new ErrorCollector();
        var items = new Dictionary<string, Item>();

        errors.NestedAgainst(Validator, items, 5);

        Assert.Empty(errors);
    }

    [Fact]
    public void NestedAgainst_WithNullDictionary_AddsNullError()
    {
        var errors = new ErrorCollector();
        IReadOnlyDictionary<string, Item>? items = null;

        errors.NestedAgainst(Validator, items, 5);

        var error = Assert.Single(errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}