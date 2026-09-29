using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatableAgainstExtensionsEnumerableTests
{
    [Fact]
    public void ValidateOrThrowAgainst_WithAllItemsWithinCapacity_ReturnsItems()
    {
        IEnumerable<Item> items = new[]
        {
            new Item { Size = 3 },
            new Item { Size = 5 }
        };

        var result = items.ValidateOrThrowAgainst(5);

        Assert.Equal(items, result);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithOneItemAboveCapacity_ThrowsWithIndexedPath()
    {
        IEnumerable<Item> items = new[]
        {
            new Item { Size = 3 },
            new Item { Size = 6 }
        };

        var exception = Assert.Throws<ValidationException>(() => items.ValidateOrThrowAgainst(5));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("items[1].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithMultipleItemsAboveCapacity_ThrowsWithAllIndexedPaths()
    {
        IEnumerable<Item> items = new[]
        {
            new Item { Size = 7 },
            new Item { Size = 2 },
            new Item { Size = 9 }
        };

        var exception = Assert.Throws<ValidationException>(() => items.ValidateOrThrowAgainst(5));

        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "items[0].Size" && e.Message == "Must not exceed 5.");
        Assert.Contains(exception.Errors, e => e.Path == "items[2].Size" && e.Message == "Must not exceed 5.");
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithEmptyEnumerable_ReturnsEmpty()
    {
        IEnumerable<Item> items = Array.Empty<Item>();

        var result = items.ValidateOrThrowAgainst(5);

        Assert.Empty(result);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithNullEnumerable_ThrowsWithNullError()
    {
        IEnumerable<Item>? items = null;

        var exception = Assert.Throws<ValidationException>(() => items.ValidateOrThrowAgainst(5));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithSetOfItems_ThrowsWithIndexedPath()
    {
        IReadOnlySet<Item> items = new HashSet<Item>
        {
            new() { Size = 3 },
            new() { Size = 6 }
        };

        var exception = Assert.Throws<ValidationException>(() => items.ValidateOrThrowAgainst(5));

        var error = Assert.Single(exception.Errors);
        Assert.StartsWith("items[", error.Path);
        Assert.EndsWith("].Size", error.Path);
    }

    [Fact]
    public void ValidateAgainst_WithItemAboveCapacity_ReturnsErrorWithoutThrowing()
    {
        IEnumerable<Item> items = new[] { new Item { Size = 6 } };

        var errors = items.ValidateAgainst(5);

        var error = Assert.Single(errors);
        Assert.Equal("items[0].Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateAgainst_WithNullEnumerable_ReturnsNullError()
    {
        IEnumerable<Item>? items = null;

        var errors = items.ValidateAgainst(5);

        var error = Assert.Single(errors);
        Assert.Equal("items", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}