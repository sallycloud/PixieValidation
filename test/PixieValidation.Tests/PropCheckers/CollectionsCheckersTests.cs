using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class CollectionsCheckersTests
{
    private static readonly PropChecker<string> NotEmptyChecker = value =>
        string.IsNullOrEmpty(value) ? "Value is required." : null;
    
    [Fact]
    public void FirstInvalid_WithAllValidItems_ReturnsNoError()
    {
        var checker = CollectionCheckers.FirstInvalid(NotEmptyChecker);

        var error = checker(["Alice", "Bob"]);

        Assert.Null(error);
    }

    [Fact]
    public void FirstInvalid_WithEmptySequence_ReturnsNoError()
    {
        var checker = CollectionCheckers.FirstInvalid(NotEmptyChecker);

        var error = checker(Array.Empty<string>());

        Assert.Null(error);
    }

    [Fact]
    public void FirstInvalid_WithOneInvalidItem_ReturnsErrorWithValue()
    {
        var checker = CollectionCheckers.FirstInvalid(NotEmptyChecker);

        var error = checker(["Alice", "", "Bob"]);

        Assert.Equal("Value is required. ()", error);
    }

    [Fact]
    public void FirstInvalid_WithMultipleInvalidItems_ReturnsOnlyFirstError()
    {
        var checker = CollectionCheckers.FirstInvalid(NotEmptyChecker);

        var error = checker(["", "", "Bob"]);

        Assert.Equal("Value is required. ()", error);
    }

    [Fact]
    public void FirstInvalid_WithNonEmptyOffendingValue_IncludesValueInMessage()
    {
        PropChecker<string> minLengthChecker = value =>
            value.Length < 5 ? "Must have at least 5 characters." : null;
        var checker = CollectionCheckers.FirstInvalid(minLengthChecker);

        var error = checker(["Alice", "Bob"]);

        Assert.Equal("Must have at least 5 characters. (Bob)", error);
    }
    
    [Fact]
    public void FirstInvalid_WithNullSequence_ReturnsNullMessage()
    {
        var checker = CollectionCheckers.FirstInvalid(NotEmptyChecker);

        var error = checker(null);

        Assert.Equal("Must not be null.", error);
    }
}