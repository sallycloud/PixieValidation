using PixieValidation.Domain;
using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatorExtensionsDictionaryTests
{
    private static readonly PersonValidator Validator = new();

    [Fact]
    public void ValidateOrThrow_WithAllValidPersons_ReturnsPersons()
    {
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>
        {
            ["Captain"] = new Person { Name = "Alice", Age = 30 },
            ["Coach"] = new Person { Name = "Bob", Age = 25 }
        };

        var result = Validator.ValidateOrThrow(people);

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

        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateOrThrow(people));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("[Coach].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithEmptyDictionary_ReturnsPersons()
    {
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>();

        var result = Validator.ValidateOrThrow(people);

        Assert.Same(people, result);
    }
    
    [Fact]
    public void ValidateOrThrow_WithNullDictionary_ThrowsWithNullError()
    {
        IReadOnlyDictionary<string, Person>? people = null;

        var exception = Assert.Throws<ValidationException>(() => Validator.ValidateOrThrow(people));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}