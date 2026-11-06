using PixieValidation.Domain;
using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatorExtensionsSingleTests
{
    private static readonly PersonValidator Validator = new();

    [Fact]
    public void Validate_WithValidPerson_ReturnsNoErrors()
    {
        var person = new Person { Name = "Alice", Age = 30 };

        var errors = Validator.Validate(person);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WithInvalidPerson_ReturnsErrorWithoutThrowing()
    {
        var person = new Person { Name = "", Age = 30 };

        var errors = Validator.Validate(person);

        var error = Assert.Single(errors);
        Assert.Equal("Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void Validate_WithGivenCollector_AddsToItAndReturnsIt()
    {
        var collector = new ErrorCollector();
        var person = new Person { Name = "", Age = 30 };

        var result = Validator.Validate(person, collector);

        Assert.Same(collector, result);
        Assert.Equal("Name", Assert.Single(result).Path);
    }

    [Fact]
    public void ValidateOrThrow_WithValidPerson_ReturnsSamePerson()
    {
        var person = new Person { Name = "Alice", Age = 30 };

        var result = Validator.ValidateOrThrow(person);

        Assert.Same(person, result);
    }

    [Fact]
    public void ValidateOrThrow_WithInvalidPerson_ThrowsWithPath()
    {
        var person = new Person { Name = "", Age = 30 };

        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateOrThrow(person));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }
}
