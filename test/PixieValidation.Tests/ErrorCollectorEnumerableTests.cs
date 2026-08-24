using PixieValidation.Domain;
using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorEnumerableTests
{
    [Fact]
    public void Nested_WithInvalidEnumerableOfChildren_AddsErrorsWithIndexedPaths()
    {
        var errors = new ErrorCollector();
        IEnumerable<Person> people = new[]
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "", Age = 200 }
        };

        errors.Nested(people);

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, e => e.Path == "people[1].Name" && e.Message == "Value is required.");
        Assert.Contains(list, e => e.Path == "people[1].Age" && e.Message == "Must be between 0 and 150.");
    }

    [Fact]
    public void Nested_WithValidatorAndInvalidEnumerableOfChildren_AddsErrorsWithIndexedPaths()
    {
        var errors = new ErrorCollector();
        var validator = new PersonValidator();
        IEnumerable<Person> people = new[]
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "", Age = 200 }
        };

        errors.Nested(validator, people);

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, e => e.Path == "people[1].Name" && e.Message == "Value is required.");
        Assert.Contains(list, e => e.Path == "people[1].Age" && e.Message == "Must be between 0 and 150.");
    }
}