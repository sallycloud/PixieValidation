using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatableAgainstExtensionsDictionaryTests
{
    [Fact]
    public void ValidateOrThrowAgainst_WithAllItemsWithinCapacity_ReturnsItems()
    {
        IReadOnlyDictionary<string, Item> items = new Dictionary<string, Item>
        {
            ["Left"] = new Item { Size = 3 },
            ["Right"] = new Item { Size = 5 }
        };

        var result = items.ValidateOrThrowAgainst(5);

        Assert.Same(items, result);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithOneItemAboveCapacity_ThrowsWithKeyedPath()
    {
        IReadOnlyDictionary<string, Item> items = new Dictionary<string, Item>
        {
            ["Left"] = new Item { Size = 6 },
            ["Right"] = new Item { Size = 3 }
        };

        var exception = Assert.Throws<ValidationException>(() => items.ValidateOrThrowAgainst(5));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("items[Left].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithMultipleItemsAboveCapacity_ThrowsWithAllKeyedPaths()
    {
        IReadOnlyDictionary<string, Item> items = new Dictionary<string, Item>
        {
            ["Left"] = new Item { Size = 7 },
            ["Middle"] = new Item { Size = 2 },
            ["Right"] = new Item { Size = 9 }
        };

        var exception = Assert.Throws<ValidationException>(() => items.ValidateOrThrowAgainst(5));

        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "items[Left].Size" && e.Message == "Must not exceed 5.");
        Assert.Contains(exception.Errors, e => e.Path == "items[Right].Size" && e.Message == "Must not exceed 5.");
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithEmptyDictionary_ReturnsItems()
    {
        IReadOnlyDictionary<string, Item> items = new Dictionary<string, Item>();

        var result = items.ValidateOrThrowAgainst(5);

        Assert.Same(items, result);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithNullDictionary_ThrowsWithNullError()
    {
        IReadOnlyDictionary<string, Item>? items = null;

        var exception = Assert.Throws<ValidationException>(() => items.ValidateOrThrowAgainst(5));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    [Fact]
    public void ValidateAgainst_WithItemAboveCapacity_ReturnsErrorWithoutThrowing()
    {
        IReadOnlyDictionary<string, Item> items = new Dictionary<string, Item>
        {
            ["Left"] = new Item { Size = 6 }
        };

        var errors = items.ValidateAgainst(5);

        var error = Assert.Single(errors);
        Assert.Equal("items[Left].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateAgainst_WithNullDictionary_ReturnsNullError()
    {
        IReadOnlyDictionary<string, Item>? items = null;

        var errors = items.ValidateAgainst(5);

        var error = Assert.Single(errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}