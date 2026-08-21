namespace PixieValidation.Tests;

public class CollectionValidationHelperTests
{
    private static readonly PropChecker<string> NotEmptyChecker = value =>
        string.IsNullOrEmpty(value) ? "Value is required." : null;

    [Fact]
    public void CollectErrors_WithEmptyCollection_ReturnsNoErrors()
    {
        string[] names = [];

    var errors = CollectionValidationHelper.CollectErrors(
            names, NotEmptyChecker, nameof(names));

        Assert.Empty(errors);
    }

    [Fact]
    public void CollectErrors_WithAllValidValues_ReturnsNoErrors()
    {
        string[] names = ["Alice", "Bob"];

        var errors = CollectionValidationHelper.CollectErrors(
            names, NotEmptyChecker, nameof(names));

        Assert.Empty(errors);
    }

    [Fact]
    public void CollectErrors_WithOneInvalidValue_ReturnsSingleErrorWithIndexedPath()
    {
        string[] names = ["Alice", "", "Bob"];

        var errors = CollectionValidationHelper.CollectErrors(
            names, NotEmptyChecker, nameof(names)).ToList();

        var error = Assert.Single(errors);
        Assert.Equal("names[1]", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void CollectErrors_WithMultipleInvalidValues_ReturnsOneErrorPerInvalidValue()
    {
        string[] names = ["", "Bob", ""];

        var errors = CollectionValidationHelper.CollectErrors(
            names, NotEmptyChecker, nameof(names)).ToList();

        Assert.Equal(2, errors.Count);
        Assert.Contains(errors, e => e.Path == "names[0]" && e.Message == "Value is required.");
        Assert.Contains(errors, e => e.Path == "names[2]" && e.Message == "Value is required.");
    }
}