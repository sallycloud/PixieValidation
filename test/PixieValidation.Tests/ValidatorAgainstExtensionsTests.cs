using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatorAgainstExtensionsTests
{
    private static readonly ItemValidator Validator = new();

    // Einzelnes Objekt

    [Fact]
    public void ValidateOrThrowAgainst_WithValidItem_ReturnsSameItem()
    {
        var item = new Item { Size = 5 };

        var result = Validator.ValidateOrThrowAgainst(item, 5);

        Assert.Same(item, result);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithItemAboveCapacity_ThrowsWithPathAndMessage()
    {
        var item = new Item { Size = 6 };

        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateOrThrowAgainst(item, 5));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateAgainst_WithItemAboveCapacity_ReturnsErrorWithoutThrowing()
    {
        var item = new Item { Size = 6 };

        var errors = Validator.ValidateAgainst(item, 5);

        var error = Assert.Single(errors);
        Assert.Equal("Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    // IEnumerable<T>

    [Fact]
    public void ValidateOrThrowAgainst_WithAllItemsWithinCapacity_ReturnsItems()
    {
        IEnumerable<Item> items = new[] { new Item { Size = 3 }, new Item { Size = 5 } };

        var result = Validator.ValidateOrThrowAgainst(items, 5);

        Assert.Equal(items, result);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithOneItemAboveCapacity_ThrowsWithIndexedPath()
    {
        IEnumerable<Item> items = new[] { new Item { Size = 3 }, new Item { Size = 6 } };

        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateOrThrowAgainst(items, 5));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("items[1].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithNullEnumerable_ThrowsWithNullError()
    {
        IEnumerable<Item>? items = null;

        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateOrThrowAgainst(items, 5));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    // Dictionary

    [Fact]
    public void ValidateOrThrowAgainst_WithAllItemsWithinCapacity_ReturnsSameDictionary()
    {
        IReadOnlyDictionary<string, Item> items = new Dictionary<string, Item>
        {
            ["Left"] = new Item { Size = 3 },
            ["Right"] = new Item { Size = 5 }
        };

        var result = Validator.ValidateOrThrowAgainst(items, 5);

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

        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateOrThrowAgainst(items, 5));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("items[Left].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithNullDictionary_ThrowsWithNullError()
    {
        IReadOnlyDictionary<string, Item>? items = null;

        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateOrThrowAgainst(items, 5));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}