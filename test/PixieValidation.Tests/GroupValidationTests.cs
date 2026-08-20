using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class GroupValidationTests
{
    [Fact]
    public void ValidateOrThrow_WithMultipleErrors_ThrowsWithAllPaths()
    {
        var group = new Group
        {
            Name = "", // ungültig
            Persons =
            [
                new Person { Name = "Alice", Age = 30 },   // gültig
                new Person { Name = "", Age = 25 },        // Name ungültig
                new Person { Name = "Bob", Age = 200 }     // Age ungültig
            ]
        };

        var exception = Assert.Throws<ValidationException>(group.ValidateOrThrow);

        Assert.Equal(3, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "Name" && e.Message == "Name is required.");
        Assert.Contains(exception.Errors, e => e.Path == "Persons[1].Name" && e.Message == "Name is required.");
        Assert.Contains(exception.Errors, e => e.Path == "Persons[2].Age" && e.Message == "Age must be between 0 and 150.");
    }

    [Fact]
    public void ValidateOrThrow_WithValidGroup_DoesNotThrow()
    {
        var group = new Group
        {
            Name = "Team A",
            Persons = [new Person { Name = "Alice", Age = 30 }]
        };

        var exception = Record.Exception(group.ValidateOrThrow);

        Assert.Null(exception);
    }
}