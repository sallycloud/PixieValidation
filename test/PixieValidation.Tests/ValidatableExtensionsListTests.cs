using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatableExtensionsListTests
{
    [Fact]
    public void ValidateOrThrow_WithAllValidPersons_ReturnsPersons()
    {
        IReadOnlyList<Person> people = [
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 }
        ];

        var result = people.ValidateOrThrow();

        Assert.Same(people, result);
    }

    [Fact]
    public void ValidateOrThrow_WithOneInvalidPerson_ThrowsWithIndexedPath()
    {
        IReadOnlyList<Person> people = [
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "", Age = 25 }
        ];

        var exception = Assert.Throws<ValidationException>(() => people.ValidateOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("people[1].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithMultipleInvalidPersons_ThrowsWithAllIndexedPaths()
    {
        IReadOnlyList<Person> people = [
            new Person { Name = "", Age = 30 },
            new Person { Name = "Bob", Age = 200 }
        ];

        var exception = Assert.Throws<ValidationException>(() => people.ValidateOrThrow());

        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "people[0].Name" && e.Message == "Value is required.");
        Assert.Contains(exception.Errors, e => e.Path == "people[1].Age" && e.Message == "Must be between 0 and 150.");
    }

    [Fact]
    public void ValidateOrThrow_WithEmptyList_ReturnsPersons()
    {
        IReadOnlyList<Person> people = [];

        var result = people.ValidateOrThrow();

        Assert.Same(people, result);
    }

    [Fact]
    public void ToValidOrThrow_WithAllValidPersons_ReturnsValidWrappingPersons()
    {
        IReadOnlyList<Person> people = [
            new Person { Name = "Alice", Age = 30 }
        ];

        Valid<IReadOnlyList<Person>> result = people.ToValidOrThrow();

        Assert.Same(people, result.Value);
    }

    [Fact]
    public void ToValidOrThrow_WithOneInvalidPerson_ThrowsWithIndexedPath()
    {
        IReadOnlyList<Person> people = [
            new Person { Name = "", Age = 30 }
        ];

        var exception = Assert.Throws<ValidationException>(() => people.ToValidOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("people[0].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }
    
    [Fact]
    public void ValidateOrThrow_WithNullList_ThrowsWithNullError()
    {
        IReadOnlyList<Person>? people = null;

        var exception = Assert.Throws<ValidationException>(() => people.ValidateOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("people", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    [Fact]
    public void ToValidOrThrow_WithNullList_ThrowsWithNullError()
    {
        IReadOnlyList<Person>? people = null;

        var exception = Assert.Throws<ValidationException>(() => people.ToValidOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("people", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}