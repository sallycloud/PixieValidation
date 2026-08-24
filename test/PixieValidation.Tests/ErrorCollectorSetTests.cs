using PixieValidation.Domain;
using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorSetTests
{
    [Fact]
    public void CheckAll_WithInvalidSetValues_AddsOneErrorPerInvalidValue()
    {
        var errors = new ErrorCollector();
        PropChecker<string> notEmptyChecker = value => string.IsNullOrEmpty(value) ? "Value is required." : null;
        IReadOnlySet<string> names = new HashSet<string> { "Alice", "" };

        errors.CheckAll(names, notEmptyChecker);

        var error = Assert.Single(errors);
        Assert.StartsWith("names[", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }
    
    [Fact]
    public void Nested_WithInvalidSetOfChildren_AddsOneErrorPerInvalidChild()
    {
        var errors = new ErrorCollector();
        IReadOnlySet<Person> people = new HashSet<Person>
        {
            new() { Name = "Alice", Age = 30 },
            new() { Name = "", Age = 25 }
        };

        errors.Nested(people);

        var error = Assert.Single(errors);
        Assert.StartsWith("people[", error.Path);
        Assert.EndsWith("].Name", error.Path);
    }
    
    [Fact]
    public void Nested_WithValidatorAndInvalidSetOfChildren_AddsOneErrorPerInvalidChild()
    {
        var errors = new ErrorCollector();
        var validator = new PersonValidator();
        IReadOnlySet<Person> people = new HashSet<Person>
        {
            new() { Name = "Alice", Age = 30 },
            new() { Name = "", Age = 25 }
        };

        errors.Nested(validator, people);

        var error = Assert.Single(errors);
        Assert.StartsWith("people[", error.Path);
        Assert.EndsWith("].Name", error.Path);
    }
}