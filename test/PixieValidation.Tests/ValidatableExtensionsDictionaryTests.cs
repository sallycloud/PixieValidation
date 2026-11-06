using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatableExtensionsDictionaryTests
{
    [Fact]
    public void ValidateOrThrow_WithAllValidPersons_ReturnsPersons()
    {
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>
        {
            ["Captain"] = new Person { Name = "Alice", Age = 30 },
            ["Coach"] = new Person { Name = "Bob", Age = 25 }
        };

        var result = people.ValidateOrThrow();

        Assert.Same(people, result);
    }

    [Fact]
    public void ValidateOrThrow_WithOneInvalidPerson_ThrowsWithKeyedPath()
    {
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>
        {
            ["Captain"] = new Person { Name = "Alice", Age = 30 },
            ["Coach"] = new Person { Name = "", Age = 25 }
        };

        var exception = Assert.Throws<ValidationException>(() => people.ValidateOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("people[Coach].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithEmptyDictionary_ReturnsPersons()
    {
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>();

        var result = people.ValidateOrThrow();

        Assert.Same(people, result);
    }
    
    [Fact]
    public void ValidateOrThrow_WithNullDictionary_ThrowsWithNullError()
    {
        IReadOnlyDictionary<string, Person>? people = null;

        var exception = Assert.Throws<ValidationException>(() => people.ValidateOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("people", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
    
    [Fact]
    public void Validate_WithAllValidPersons_ReturnsNoErrors()
    {
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>
        {
            ["Captain"] = new Person { Name = "Alice", Age = 30 }
        };

        var errors = people.Validate();

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WithOneInvalidPerson_ReturnsKeyedErrorWithoutThrowing()
    {
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>
        {
            ["Captain"] = new Person { Name = "Alice", Age = 30 },
            ["Coach"] = new Person { Name = "", Age = 25 }
        };

        var errors = people.Validate();

        var error = Assert.Single(errors);
        Assert.Equal("people[Coach].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void Validate_WithNullDictionary_ReturnsNullError()
    {
        IReadOnlyDictionary<string, Person>? people = null;

        var errors = people.Validate();

        var error = Assert.Single(errors);
        Assert.Equal("people", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}
