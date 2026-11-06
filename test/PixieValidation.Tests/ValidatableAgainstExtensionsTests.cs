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
    
    [Fact]
    public void Validate_WithValidPerson_ReturnsNoErrors()
    {
        var person = new Person { Name = "Alice", Age = 30 };

        var errors = person.Validate();

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WithInvalidPerson_ReturnsAllErrorsWithoutThrowing()
    {
        var person = new Person { Name = "", Age = 200 };

        var errors = person.Validate();

        Assert.Equal(2, errors.Count());
        Assert.Contains(errors, e => e.Path == "Name" && e.Message == "Value is required.");
        Assert.Contains(errors, e => e.Path == "Age" && e.Message == "Must be between 0 and 150.");
    }

    [Fact]
    public void Validate_WithGivenCollector_AddsToItAndReturnsIt()
    {
        var collector = new ErrorCollector();
        var person = new Person { Name = "", Age = 30 };

        var result = person.Validate(collector);

        Assert.Same(collector, result);
        Assert.Equal("Name", Assert.Single(result).Path);
    }

    [Fact]
    public void ToValidOrThrow_WithInvalidPerson_Throws()
    {
        var person = new Person { Name = "", Age = 30 };

        var exception = Assert.Throws<ValidationException>(() => person.ToValidOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }
}
