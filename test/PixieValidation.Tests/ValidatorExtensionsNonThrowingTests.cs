using PixieValidation.Domain;
using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatorExtensionsNonThrowingTests
{
    private static readonly PersonValidator Validator = new();

    // IEnumerable

    [Fact]
    public void Validate_WithAllValidPersonsInEnumerable_ReturnsNoErrors()
    {
        IEnumerable<Person> people = new[]
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 }
        };

        var errors = Validator.Validate(people);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WithInvalidPersonInEnumerable_ReturnsIndexedErrorWithoutThrowing()
    {
        IEnumerable<Person> people = new[]
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "", Age = 25 }
        };

        var errors = Validator.Validate(people);

        var error = Assert.Single(errors);
        Assert.Equal("people[1].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void Validate_WithNullEnumerable_ReturnsNullError()
    {
        IEnumerable<Person>? people = null;

        var errors = Validator.Validate(people);

        var error = Assert.Single(errors);
        Assert.Equal("people", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }

    [Fact]
    public void Validate_WithEnumerableAndGivenCollector_AddsToItAndReturnsIt()
    {
        var collector = new ErrorCollector();
        IEnumerable<Person> people = new[] { new Person { Name = "", Age = 30 } };

        var result = Validator.Validate(people, collector);

        Assert.Same(collector, result);
        Assert.Single(result);
    }

    // Dictionary

    [Fact]
    public void Validate_WithAllValidPersonsInDictionary_ReturnsNoErrors()
    {
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>
        {
            ["Captain"] = new Person { Name = "Alice", Age = 30 },
            ["Coach"] = new Person { Name = "Bob", Age = 25 }
        };

        var errors = Validator.Validate(people);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WithInvalidPersonInDictionary_ReturnsKeyedErrorWithoutThrowing()
    {
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>
        {
            ["Captain"] = new Person { Name = "Alice", Age = 30 },
            ["Coach"] = new Person { Name = "", Age = 25 }
        };

        var errors = Validator.Validate(people);

        var error = Assert.Single(errors);
        Assert.Equal("people[Coach].Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void Validate_WithNullDictionary_ReturnsNullError()
    {
        IReadOnlyDictionary<string, Person>? people = null;

        var errors = Validator.Validate(people);

        var error = Assert.Single(errors);
        Assert.Equal("people", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}
