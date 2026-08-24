namespace PixieValidation.Tests;

public class PropCheckerExtensionsTests
{
    private static readonly PropChecker<string> NotEmptyChecker = value =>
        string.IsNullOrEmpty(value) ? "Value is required." : null;

    private static readonly PropChecker<string> MinLengthChecker = value =>
        value.Length < 3 ? "Must have at least 3 characters." : null;

    [Fact]
    public void ValidateOrThrow_WithValidValue_ReturnsValue()
    {
        var result = NotEmptyChecker.ValidateOrThrow("Alice");

        Assert.Equal("Alice", result);
    }

    [Fact]
    public void ValidateOrThrow_WithInvalidValue_ThrowsWithPathAndMessage()
    {
        var name = "";

        var exception = Assert.Throws<ValidationException>(() => NotEmptyChecker.ValidateOrThrow(name));

        var error = Assert.Single(exception.Errors);
        Assert.Equal("name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }

    [Fact]
    public void And_WithBothCheckersSucceeding_ReturnsNoError()
    {
        var checker = NotEmptyChecker.And(MinLengthChecker);

        var error = checker("Alice");

        Assert.Null(error);
    }

    [Fact]
    public void And_WithFirstCheckerFailing_ReturnsFirstError()
    {
        var checker = NotEmptyChecker.And(MinLengthChecker);

        var error = checker("");

        Assert.Equal("Value is required.", error);
    }

    [Fact]
    public void And_WithSecondCheckerFailing_ReturnsSecondError()
    {
        var checker = NotEmptyChecker.And(MinLengthChecker);

        var error = checker("Al");

        Assert.Equal("Must have at least 3 characters.", error);
    }

    [Fact]
    public void OptionalRef_WithNull_ReturnsNoError()
    {
        var checker = MinLengthChecker.OptionalRef();

        var error = checker(null);

        Assert.Null(error);
    }

    [Fact]
    public void OptionalRef_WithValidValue_ReturnsNoError()
    {
        var checker = MinLengthChecker.OptionalRef();

        var error = checker("Alice");

        Assert.Null(error);
    }

    [Fact]
    public void OptionalRef_WithInvalidValue_ReturnsError()
    {
        var checker = MinLengthChecker.OptionalRef();

        var error = checker("Al");

        Assert.Equal("Must have at least 3 characters.", error);
    }

    [Fact]
    public void OptionalVal_WithNull_ReturnsNoError()
    {
        PropChecker<int> positiveChecker = value => value <= 0 ? "Must be positive." : null;
        var checker = positiveChecker.OptionalVal();

        var error = checker(null);

        Assert.Null(error);
    }

    [Fact]
    public void OptionalVal_WithValidValue_ReturnsNoError()
    {
        PropChecker<int> positiveChecker = value => value <= 0 ? "Must be positive." : null;
        var checker = positiveChecker.OptionalVal();

        var error = checker(5);

        Assert.Null(error);
    }

    [Fact]
    public void OptionalVal_WithInvalidValue_ReturnsError()
    {
        PropChecker<int> positiveChecker = value => value <= 0 ? "Must be positive." : null;
        var checker = positiveChecker.OptionalVal();

        var error = checker(-1);

        Assert.Equal("Must be positive.", error);
    }

    [Fact]
    public void When_WithConditionTrue_AppliesChecker()
    {
        var checker = NotEmptyChecker.When(value => value != "skip");

        var error = checker("");

        Assert.Equal("Value is required.", error);
    }

    [Fact]
    public void When_WithConditionFalse_SkipsChecker()
    {
        var checker = NotEmptyChecker.When(value => value != "skip");

        var error = checker("skip");

        Assert.Null(error);
    }

    [Fact]
    public void RequiredRef_WithNull_ReturnsDefaultMessage()
    {
        var checker = MinLengthChecker.RequiredRef();

        var error = checker(null);

        Assert.Equal("Value is required.", error);
    }

    [Fact]
    public void RequiredRef_WithNull_AndCustomMessage_ReturnsCustomMessage()
    {
        var checker = MinLengthChecker.RequiredRef("Name is required.");

        var error = checker(null);

        Assert.Equal("Name is required.", error);
    }

    [Fact]
    public void RequiredRef_WithValidNonNullValue_ReturnsNoError()
    {
        var checker = MinLengthChecker.RequiredRef();

        var error = checker("Alice");

        Assert.Null(error);
    }

    [Fact]
    public void RequiredRef_WithInvalidNonNullValue_DelegatesToWrappedChecker()
    {
        var checker = MinLengthChecker.RequiredRef();

        var error = checker("Al");

        Assert.Equal("Must have at least 3 characters.", error);
    }
}