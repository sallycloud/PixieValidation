using PixieValidation.Domain;
using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorDictionaryTests
{
    [Fact]
    public void Nested_WithInvalidDictionaryOfChildren_AddsErrorsWithKeyedPaths()
    {
        var errors = new ErrorCollector();
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>
        {
            ["Captain"] = new Person { Name = "Alice", Age = 30 },
            ["Coach"] = new Person { Name = "", Age = 200 }
        };

        errors.Nested(people);

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, e => e.Path == "people[Coach].Name" && e.Message == "Value is required.");
        Assert.Contains(list, e => e.Path == "people[Coach].Age" && e.Message == "Must be between 0 and 150.");
    }

    [Fact]
    public void Nested_WithValidatorAndInvalidDictionaryOfChildren_AddsErrorsWithKeyedPaths()
    {
        var errors = new ErrorCollector();
        var validator = new PersonValidator();
        IReadOnlyDictionary<string, Person> people = new Dictionary<string, Person>
        {
            ["Captain"] = new Person { Name = "Alice", Age = 30 },
            ["Coach"] = new Person { Name = "", Age = 200 }
        };

        errors.Nested(validator, people);

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, e => e.Path == "people[Coach].Name" && e.Message == "Value is required.");
        Assert.Contains(list, e => e.Path == "people[Coach].Age" && e.Message == "Must be between 0 and 150.");
    }
}