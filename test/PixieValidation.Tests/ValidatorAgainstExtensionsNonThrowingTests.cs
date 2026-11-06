using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatorAgainstExtensionsNonThrowingTests
{
    private static readonly ItemValidator Validator = new();

    // IEnumerable

    [Fact]
    public void ValidateAgainst_WithAllItemsInEnumerableWithinCapacity_ReturnsNoErrors()
    {
        IEnumerable<Item> items = new[] { new Item { Size = 3 }, new Item { Size = 5 } };

        var errors = Validator.ValidateAgainst(items, 5);

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateAgainst_WithItemInEnumerableAboveCapacity_ReturnsIndexedErrorWithoutThrowing()
    {
        IEnumerable<Item> items = new[] { new Item { Size = 3 }, new Item { Size = 6 } };

        var errors = Validator.ValidateAgainst(items, 5);

        var error = Assert.Single(errors);
        Assert.Equal("items[1].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateAgainst_WithNullEnumerable_ReturnsNullError()
    {
        IEnumerable<Item>? items = null;

        var errors = Validator.ValidateAgainst(items, 5);

        var error = Assert.Single(errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    // Dictionary

    [Fact]
    public void ValidateAgainst_WithAllItemsInDictionaryWithinCapacity_ReturnsNoErrors()
    {
        IReadOnlyDictionary<string, Item> items = new Dictionary<string, Item>
        {
            ["Left"] = new Item { Size = 3 },
            ["Right"] = new Item { Size = 5 }
        };

        var errors = Validator.ValidateAgainst(items, 5);

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateAgainst_WithItemInDictionaryAboveCapacity_ReturnsKeyedErrorWithoutThrowing()
    {
        IReadOnlyDictionary<string, Item> items = new Dictionary<string, Item>
        {
            ["Left"] = new Item { Size = 6 },
            ["Right"] = new Item { Size = 3 }
        };

        var errors = Validator.ValidateAgainst(items, 5);

        var error = Assert.Single(errors);
        Assert.Equal("items[Left].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateAgainst_WithNullDictionary_ReturnsNullError()
    {
        IReadOnlyDictionary<string, Item>? items = null;

        var errors = Validator.ValidateAgainst(items, 5);

        var error = Assert.Single(errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}
