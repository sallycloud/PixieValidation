using PixieValidation.Domain;
using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatorExtensionsSetTests
{
    private static readonly PersonValidator Validator = new();

    [Fact]
    public void ValidateOrThrow_WithAllValidPersons_ReturnsPersons()
    {
        IReadOnlySet<Person> people = new HashSet<Person>
        {
            new() { Name = "Alice", Age = 30 },
            new() { Name = "Bob", Age = 25 }
        };

        var result = Validator.ValidateOrThrow(people);

        Assert.Same(people, result);
    }

    [Fact]
    public void ValidateOrThrow_WithOneInvalidPerson_ThrowsWithIndexedPath()
    {
        IReadOnlySet<Person> people = new HashSet<Person>
        {
            new() { Name = "Alice", Age = 30 },
            new() { Name = "", Age = 25 }
        };

        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateOrThrow(people));

        var error = Assert.Single(exception.Errors);
        Assert.StartsWith("people[", error.Path);
        Assert.EndsWith("].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithMultipleInvalidPersons_ThrowsWithAllErrors()
    {
        IReadOnlySet<Person> people = new HashSet<Person>
        {
            new() { Name = "", Age = 30 },
            new() { Name = "Bob", Age = 200 }
        };

        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateOrThrow(people));

        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path.EndsWith("].Name") && e.Message == "Value is required.");
        Assert.Contains(exception.Errors, e => e.Path.EndsWith("].Age") && e.Message == "Must be between 0 and 150.");
    }

    [Fact]
    public void ValidateOrThrow_WithEmptySet_ReturnsPersons()
    {
        IReadOnlySet<Person> people = new HashSet<Person>();

        var result = Validator.ValidateOrThrow(people);

        Assert.Same(people, result);
    }

    [Fact]
    public void ToValidOrThrow_WithAllValidPersons_ReturnsValidWrappingPersons()
    {
        IReadOnlySet<Person> people = new HashSet<Person>
        {
            new() { Name = "Alice", Age = 30 }
        };

        Valid<IReadOnlySet<Person>> result = Validator.ToValidOrThrow(people);

        Assert.Same(people, result.Value);
    }

    [Fact]
    public void ToValidOrThrow_WithOneInvalidPerson_ThrowsWithIndexedPath()
    {
        IReadOnlySet<Person> people = new HashSet<Person>
        {
            new() { Name = "", Age = 30 }
        };

        var exception = Assert.Throws<ValidationException>(() => Validator.ToValidOrThrow(people));

        var error = Assert.Single(exception.Errors);
        Assert.StartsWith("people[", error.Path);
        Assert.EndsWith("].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }
}