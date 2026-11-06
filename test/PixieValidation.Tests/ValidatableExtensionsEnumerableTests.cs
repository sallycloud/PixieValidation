using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatableExtensionsEnumerableTests
{
    // Validate

    [Fact]
    public void Validate_WithAllValidPersons_ReturnsNoErrors()
    {
        IEnumerable<Person> people = new[]
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 }
        };

        var errors = people.Validate();

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WithOneInvalidPerson_ReturnsIndexedErrorWithoutThrowing()
    {
        IEnumerable<Person> people = new[]
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "", Age = 25 }
        };

        var errors = people.Validate();

        var error = Assert.Single(errors);
        Assert.Equal("people[1].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void Validate_WithNullEnumerable_ReturnsNullError()
    {
        IEnumerable<Person>? people = null;

        var errors = people.Validate();

        var error = Assert.Single(errors);
        Assert.Equal("people", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    // ValidateOrThrow

    [Fact]
    public void ValidateOrThrow_WithAllValidPersons_ReturnsAllPersons()
    {
        IEnumerable<Person> people = new[]
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 }
        };

        var result = people.ValidateOrThrow();

        Assert.Equal(people, result);
    }

    [Fact]
    public void ValidateOrThrow_WithOneInvalidPerson_ThrowsWithIndexedPath()
    {
        IEnumerable<Person> people = new[]
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "", Age = 25 }
        };

        var exception = Assert.Throws<ValidationException>(() => people.ValidateOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("people[1].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithNullEnumerable_ThrowsWithNullError()
    {
        IEnumerable<Person>? people = null;

        var exception = Assert.Throws<ValidationException>(() => people.ValidateOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("people", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithLazySequence_EnumeratesItOnlyOnce()
    {
        var enumerations = 0;
        IEnumerable<Person> people = Generate(() => enumerations++);

        var result = people.ValidateOrThrow();

        Assert.Equal(1, enumerations);
        Assert.Equal(2, result.Count());
    }

    private static IEnumerable<Person> Generate(Action onEnumerate)
    {
        onEnumerate();
        yield return new Person { Name = "Alice", Age = 30 };
        yield return new Person { Name = "Bob", Age = 25 };
    }
}
