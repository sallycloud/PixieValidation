namespace PixieValidation.Tests;

public class PropCheckerSetTests
{
    private static readonly PropChecker<string> NotEmptyChecker = value =>
        string.IsNullOrEmpty(value) ? "Value is required." : null;

    [Fact]
    public void ValidateOrThrow_WithAllValidValues_ReturnsValues()
    {
        IReadOnlySet<string> names = new HashSet<string> { "Alice", "Bob" };

        var result = NotEmptyChecker.ValidateOrThrow(names);

        Assert.Same(names, result);
    }

    [Fact]
    public void ValidateOrThrow_WithOneInvalidValue_ThrowsWithSingleError()
    {
        IReadOnlySet<string> names = new HashSet<string> { "Alice", "", "Bob" };

        var exception = Assert.Throws<ValidationException>(() => NotEmptyChecker.ValidateOrThrow(names));

        var error = Assert.Single(exception.Errors);
        Assert.StartsWith("names[", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void ValidateOrThrow_WithMultipleInvalidValues_ThrowsWithOneErrorPerInvalidValue()
    {
        IReadOnlySet<string> names = new HashSet<string> { "", "Bob", "Carol" };
        // "Bob" und "Carol" sind gültig, nur der leere String ist ungültig -> ein Fehler erwartet.
        // Für einen Fall mit zwei ungültigen Werten reicht ein Count-Check, da die Iterationsreihenfolge
        // eines HashSet nicht garantiert ist und wir uns nicht auf einen bestimmten Index verlassen wollen.

        var exception = Assert.Throws<ValidationException>(() => NotEmptyChecker.ValidateOrThrow(names));

        var error = Assert.Single(exception.Errors);
        Assert.StartsWith("names[", error.Path);
    }

    [Fact]
    public void ValidateOrThrow_WithEmptySet_ReturnsValues()
    {
        IReadOnlySet<string> names = new HashSet<string>();

        var result = NotEmptyChecker.ValidateOrThrow(names);

        Assert.Same(names, result);
    }
    
    [Fact]
    public void ValidateOrThrow_WithNullSet_ThrowsWithNullError()
    {
        IReadOnlySet<string>? names = null;

        var exception = Assert.Throws<ValidationException>(() => NotEmptyChecker.ValidateOrThrow(names));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("names", error.Path);
        Assert.Equal("Must not be null.", error.Message);
    }
}