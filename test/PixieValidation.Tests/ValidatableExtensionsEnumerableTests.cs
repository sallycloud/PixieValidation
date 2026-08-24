using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatableExtensionsEnumerableTests
{
    [Fact]
    public void ValidateOrThrow_WithAllValidPersons_ReturnsPersons()
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
        Assert.Equal("[1].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithEmptyEnumerable_ReturnsPersons()
    {
        IEnumerable<Person> people = Array.Empty<Person>();

        var result = people.ValidateOrThrow();

        Assert.Empty(result);
    }
}