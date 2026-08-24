using PixieValidation.Domain;
using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorTests
{
    [Fact]
    public void Check_WithNullError_DoesNotAddError()
    {
        var errors = new ErrorCollector();

        errors.Check("Name", null);

        Assert.Empty(errors);
    }

    [Fact]
    public void Check_WithError_AddsErrorWithGivenPath()
    {
        var errors = new ErrorCollector();

        errors.Check("Name", "Value is required.");

        var error = Assert.Single(errors);
        Assert.Equal("Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void Check_CalledMultipleTimes_AddsAllErrorsInOrder()
    {
        var errors = new ErrorCollector();

        errors.Check("Name", "Value is required.");
        errors.Check("Age", null);
        errors.Check("Email", "Must be a valid email address.");

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Equal("Name", list[0].Path);
        Assert.Equal("Email", list[1].Path);
    }
    
    [Fact]
    public void Check_WithValidValue_DoesNotAddError()
    {
        var errors = new ErrorCollector();
        PropChecker<string> notEmptyChecker = value => string.IsNullOrEmpty(value) ? "Value is required." : null;
        var name = "Alice";

        errors.Check(name, notEmptyChecker);

        Assert.Empty(errors);
    }

    [Fact]
    public void Check_WithInvalidValue_AddsErrorWithInferredPropertyName()
    {
        var errors = new ErrorCollector();
        PropChecker<string> notEmptyChecker = value => string.IsNullOrEmpty(value) ? "Value is required." : null;
        var name = "";

        errors.Check(name, notEmptyChecker);

        var error = Assert.Single(errors);
        Assert.Equal("name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void Check_WithExplicitPropertyName_UsesGivenName()
    {
        var errors = new ErrorCollector();
        PropChecker<string> notEmptyChecker = value => string.IsNullOrEmpty(value) ? "Value is required." : null;

        errors.Check("", notEmptyChecker, "CustomName");

        var error = Assert.Single(errors);
        Assert.Equal("CustomName", error.Path);
    }

    [Fact]
    public void Nested_WithValidChild_DoesNotAddErrors()
    {
        var errors = new ErrorCollector();
        var person = new Person { Name = "Alice", Age = 30 };

        errors.Nested(person);

        Assert.Empty(errors);
    }

    [Fact]
    public void Nested_WithInvalidChild_AddsErrorsWithPrefixedPath()
    {
        var errors = new ErrorCollector();
        var person = new Person { Name = "", Age = 200 };

        errors.Nested(person);

        var list = errors.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, e => e.Path == "person.Name" && e.Message == "Value is required.");
        Assert.Contains(list, e => e.Path == "person.Age" && e.Message == "Must be between 0 and 150.");
    }

    [Fact]
    public void Nested_WithExplicitPropertyName_UsesGivenName()
    {
        var errors = new ErrorCollector();
        var person = new Person { Name = "", Age = 30 };

        errors.Nested(person, "CustomPerson");

        var error = Assert.Single(errors);
        Assert.Equal("CustomPerson.Name", error.Path);
    }

    [Fact]
    public void Nested_PopsPathAfterValidation_SubsequentChecksUseOriginalPath()
    {
        var errors = new ErrorCollector();
        var person = new Person { Name = "", Age = 30 };

        errors.Nested(person);
        errors.Check("Other", "Some error.");

        Assert.Contains(errors, e => e.Path == "Other");
    }
    
    
    
    [Fact]
    public void GetEnumerator_WithNoErrors_YieldsNothing()
    {
        var errors = new ErrorCollector();

        Assert.Empty(errors);
    }

    [Fact]
    public void GetEnumerator_YieldsErrorsInInsertionOrder()
    {
        var errors = new ErrorCollector();

        errors.Check("First", "Error 1.");
        errors.Check("Second", "Error 2.");
        errors.Check("Third", "Error 3.");

        var list = errors.ToList();
        Assert.Equal(["First", "Second", "Third"], list.Select(e => e.Path));
    }

    [Fact]
    public void GetEnumerator_AsNonGenericIEnumerable_YieldsSameErrors()
    {
        var errors = new ErrorCollector();
        errors.Check("Name", "Value is required.");

        var nonGeneric = (System.Collections.IEnumerable)errors;
        var items = nonGeneric.Cast<ValidationError>().ToList();

        var error = Assert.Single(items);
        Assert.Equal("Name", error.Path);
    }
}