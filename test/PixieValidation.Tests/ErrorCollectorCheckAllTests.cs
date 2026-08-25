using PixieValidation.Tests.Domain;

namespace PixieValidation.Tests;

public class ErrorCollectorCheckAllTests
{
    [Fact]
    public void ValidateOrThrow_WithAllValidMemberNames_DoesNotThrow()
    {
        var club = new Club
        {
            Name = "Chess Club",
            MemberNames = ["Alice", "Bob"]
        };

        var exception = Record.Exception(() => club.ValidateOrThrow());

        Assert.Null(exception);
    }

    [Fact]
    public void ValidateOrThrow_WithOneInvalidMemberName_ThrowsWithIndexedPath()
    {
        var club = new Club
        {
            Name = "Chess Club",
            MemberNames = ["Alice", "", "Bob"]
        };

        var exception = Assert.Throws<ValidationException>(() => club.ValidateOrThrow());

        var error = Assert.Single(exception.Errors);
        Assert.Equal("MemberNames[1]", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithMultipleInvalidMemberNames_ThrowsWithAllIndexedPaths()
    {
        var club = new Club
        {
            Name = "Chess Club",
            MemberNames = ["", "Bob", ""]
        };

        var exception = Assert.Throws<ValidationException>(() => club.ValidateOrThrow());

        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "MemberNames[0]" && e.Message == "Value is required.");
        Assert.Contains(exception.Errors, e => e.Path == "MemberNames[2]" && e.Message == "Value is required.");
    }

    [Fact]
    public void ValidateOrThrow_WithInvalidNameAndInvalidMemberName_ThrowsWithBothErrors()
    {
        var club = new Club
        {
            Name = "",
            MemberNames = ["Alice", ""]
        };

        var exception = Assert.Throws<ValidationException>(() => club.ValidateOrThrow());

        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "Name" && e.Message == "Value is required.");
        Assert.Contains(exception.Errors, e => e.Path == "MemberNames[1]" && e.Message == "Value is required.");
    }

    [Fact]
    public void ValidateOrThrow_WithEmptyMemberNames_DoesNotThrow()
    {
        var club = new Club
        {
            Name = "Chess Club",
            MemberNames = []
        };

        var exception = Record.Exception(() => club.ValidateOrThrow());

        Assert.Null(exception);
    }
    
    [Fact]
    public void CheckAll_WithNullCollection_AddsNullError()
    {
        var errors = new ErrorCollector();
        PropChecker<string> notEmptyChecker = value => string.IsNullOrEmpty(value) ? "Value is required." : null;
        IReadOnlyCollection<string>? names = null;

        errors.CheckAll(names, notEmptyChecker);

        var error = Assert.Single(errors);
        Assert.Equal("names", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}