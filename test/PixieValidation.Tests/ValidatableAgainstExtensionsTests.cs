using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatableAgainstExtensionsTests
{
    [Fact]
    public void ValidateOrThrowAgainst_WithValidItem_ReturnsSameItem()
    {
        var item = new Item { Size = 5 };

        var result = item.ValidateOrThrowAgainst(5);

        Assert.Same(item, result);
    }

    [Fact]
    public void ValidateOrThrowAgainst_WithItemAboveCapacity_ThrowsWithPathAndMessage()
    {
        var item = new Item { Size = 6 };

        var exception = Assert.Throws<ValidationException>(() => item.ValidateOrThrowAgainst(5));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateAgainst_WithValidItem_ReturnsEmptyCollector()
    {
        var item = new Item { Size = 5 };

        var errors = item.ValidateAgainst(5);

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateAgainst_WithItemAboveCapacity_ReturnsErrorWithoutThrowing()
    {
        var item = new Item { Size = 6 };

        var errors = item.ValidateAgainst(5);

        var error = Assert.Single(errors);
        Assert.Equal("Size", error.Path);
        Assert.Equal("Must not exceed 5.", error.Message);
    }

    [Fact]
    public void ValidateAgainst_WithGivenCollector_AddsErrorsToThatCollector()
    {
        var collector = new ErrorCollector();
        collector.Check("Other", "Some error.");
        var item = new Item { Size = 6 };

        var result = item.ValidateAgainst(5, collector);

        Assert.Same(collector, result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, e => e.Path == "Size");
    }
}