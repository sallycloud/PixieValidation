using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ValidatorExtensionsTest
{
    [Fact]
    public void ValidateOrThrow_WithMultipleErrors_ThrowsWithAllPaths()
    {
        var validator = new GroupValidator();
        var group = new Group
        {
            Name = "",
            Persons =
            [
                new Person { Name = "Alice", Age = 30 },
                new Person { Name = "", Age = 25 },
                new Person { Name = "Bob", Age = 200 }
            ]
        };

        var exception = Assert.Throws<ValidationException>(() => validator.ValidateOrThrow(group));

        Assert.Equal(3, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "Name" && e.Message == "Value is required.");
        Assert.Contains(exception.Errors, e => e.Path == "Persons[1].Name" && e.Message == "Value is required.");
        Assert.Contains(exception.Errors, e => e.Path == "Persons[2].Age" && e.Message == "Must be between 0 and 150.");
    }

    [Fact]
    public void ValidateOrThrow_WithValidGroup_DoesNotThrow()
    {
        var validator = new GroupValidator();
        var group = new Group
        {
            Name = "Team A",
            Persons = [new Person { Name = "Alice", Age = 30 }]
        };

        var exception = Record.Exception(() => validator.ValidateOrThrow(group));

        Assert.Null(exception);
    }

    [Fact]
    public void ToValidOrThrow_WithValidGroup_ReturnsValidWrappingGroup()
    {
        var validator = new GroupValidator();
        var group = new Group
        {
            Name = "Team A",
            Persons = [new Person { Name = "Alice", Age = 30 }]
        };

        Valid<Group> result = validator.ToValidOrThrow(group);

        Assert.Same(group, result.Value);
    }
}