namespace PixieValidation.Tests;

public class PropCheckerListTests
{
    private static readonly PropChecker<string> NotEmptyChecker = value =>
        string.IsNullOrEmpty(value) ? "Value is required." : null;

    [Fact]
    public void ValidateOrThrow_WithAllValidValues_ReturnsValues()
    {
        IReadOnlyList<string> names = ["Alice", "Bob"];

        var result = NotEmptyChecker.ValidateOrThrow(names);

        Assert.Same(names, result);
    }

    [Fact]
    public void ValidateOrThrow_WithOneInvalidValue_ThrowsWithIndexedPath()
    {
        IReadOnlyList<string> names = ["Alice", "", "Bob"];

        var exception = Assert.Throws<ValidationException>(() => NotEmptyChecker.ValidateOrThrow(names));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("names[1]", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithMultipleInvalidValues_ThrowsWithAllIndexedPaths()
    {
        IReadOnlyList<string> names = ["", "Bob", ""];

        var exception = Assert.Throws<ValidationException>(() => NotEmptyChecker.ValidateOrThrow(names));

        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "names[0]" && e.Message == "Value is required.");
        Assert.Contains(exception.Errors, e => e.Path == "names[2]" && e.Message == "Value is required.");
    }

    [Fact]
    public void ValidateOrThrow_WithEmptyList_ReturnsValues()
    {
        IReadOnlyList<string> names = [];

        var result = NotEmptyChecker.ValidateOrThrow(names);

        Assert.Same(names, result);
    }
    
    [Fact]
    public void ToValidOrThrow_WithAllValidValues_ReturnsValidWrappingValues()
    {
        IReadOnlyList<string> names = ["Alice", "Bob"];

        Valid<IReadOnlyList<string>> result = NotEmptyChecker.ToValidOrThrow(names);

        Assert.Same(names, result.Value);
    }

    [Fact]
    public void ToValidOrThrow_WithOneInvalidValue_ThrowsWithIndexedPath()
    {
        IReadOnlyList<string> names = ["Alice", "", "Bob"];

        var exception = Assert.Throws<ValidationException>(() => NotEmptyChecker.ToValidOrThrow(names));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("names[1]", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ToValidOrThrow_WithMultipleInvalidValues_ThrowsWithAllIndexedPaths()
    {
        IReadOnlyList<string> names = ["", "Bob", ""];

        var exception = Assert.Throws<ValidationException>(() => NotEmptyChecker.ToValidOrThrow(names));

        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(exception.Errors, e => e.Path == "names[0]" && e.Message == "Value is required.");
        Assert.Contains(exception.Errors, e => e.Path == "names[2]" && e.Message == "Value is required.");
    }
}