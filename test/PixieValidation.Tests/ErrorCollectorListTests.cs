using PixieValidation.Domain;
using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorListTests
{
    [Fact]
    public void CheckAll_WithAllValidListValues_DoesNotAddErrors()
    {
        var errors = new ErrorCollector();
        PropChecker<string> notEmptyChecker = value => string.IsNullOrEmpty(value) ? "Value is required." : null;
        IReadOnlyList<string> names = ["Alice", "Bob"];

        errors.CheckAll(names, notEmptyChecker);

        Assert.Empty(errors);
    }

    [Fact]
    public void CheckAll_WithInvalidListValues_AddsErrorsWithIndexedPaths()
    {
        var errors = new ErrorCollector();
        PropChecker<string> notEmptyChecker = value => string.IsNullOrEmpty(value) ? "Value is required." : null;
        IReadOnlyList<string> names = ["Alice", "", "Bob", ""];

        errors.CheckAll(names, notEmptyChecker);

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Equal("names[1]", list[0].Path);
        Assert.Equal("names[3]", list[1].Path);
    }
    
    [Fact]
    public void Nested_WithValidListOfChildren_DoesNotAddErrors()
    {
        var errors = new ErrorCollector();
        IReadOnlyList<Person> people = [
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 }
        ];

        errors.Nested(people);

        Assert.Empty(errors);
    }

    [Fact]
    public void Nested_WithInvalidListOfChildren_AddsErrorsWithIndexedPaths()
    {
        var errors = new ErrorCollector();
        IReadOnlyList<Person> people = [
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "", Age = 200 }
        ];

        errors.Nested(people);

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, e => e.Path == "people[1].Name" && e.Message == "Value is required.");
        Assert.Contains(list, e => e.Path == "people[1].Age" && e.Message == "Must be between 0 and 150.");
    }

    [Fact]
    public void Nested_WithListOfChildren_UnderOuterPrefix_CombinesPaths()
    {
        var errors = new ErrorCollector();
        var group = new Group
        {
            Name = "Team A",
            Persons = [new Person { Name = "", Age = 30 }]
        };

        errors.Nested(group);

        var error = Assert.Single(errors);
        Assert.Equal("group.Persons[0].Name", error.Path);
    }
    
    [Fact]
    public void Nested_WithValidatorAndValidListOfChildren_DoesNotAddErrors()
    {
        var errors = new ErrorCollector();
        var validator = new PersonValidator();
        IReadOnlyList<Person> people = [
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 }
        ];

        errors.Nested(validator, people);

        Assert.Empty(errors);
    }
    
    [Fact]
    public void Nested_WithValidatorAndInvalidListOfChildren_AddsErrorsWithIndexedPaths()
    {
        var errors = new ErrorCollector();
        var validator = new PersonValidator();
        IReadOnlyList<Person> people = [
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "", Age = 200 }
        ];

        errors.Nested(validator, people);

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, e => e.Path == "people[1].Name" && e.Message == "Value is required.");
        Assert.Contains(list, e => e.Path == "people[1].Age" && e.Message == "Must be between 0 and 150.");
    }
    
    [Fact]
    public void Nested_WithValidatorAndListOfChildren_UnderOuterPrefix_CombinesPaths()
    {
        var errors = new ErrorCollector();
        var validator = new GroupValidator();
        var group = new Group
        {
            Name = "Team A",
            Persons = [new Person { Name = "", Age = 30 }]
        };

        errors.Nested(validator, group);

        var error = Assert.Single(errors);
        Assert.Equal("group.Persons[0].Name", error.Path);
    }
    
    [Fact]
    public void CheckAll_WithExplicitPropertyName_UsesGivenName()
    {
        var errors = new ErrorCollector();
        PropChecker<string> notEmptyChecker = value => string.IsNullOrEmpty(value) ? "Value is required." : null;
        IReadOnlyList<string> values = [""];

        errors.CheckAll(values, notEmptyChecker, "CustomNames");

        var error = Assert.Single(errors);
        Assert.Equal("CustomNames[0]", error.Path);
    }
}