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
    
    // NotEmpty

    [Fact]
    public void NotEmpty_WithEmptyCollection_ReturnsError()
    {
        var checker = CollectionCheckers.NotEmpty<int>();

        Assert.Equal("Must not be empty.", checker(Array.Empty<int>()));
    }

    [Fact]
    public void NotEmpty_WithOneItem_ReturnsNoError()
    {
        var checker = CollectionCheckers.NotEmpty<int>();

        Assert.Null(checker(new[] { 1 }));
    }

    // MinCount

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void MinCount_WithAtLeastMinimumItems_ReturnsNoError(int count)
    {
        var checker = CollectionCheckers.MinCount<int>(2);

        Assert.Null(checker(Enumerable.Range(0, count).ToArray()));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void MinCount_WithTooFewItems_ReturnsError(int count)
    {
        var checker = CollectionCheckers.MinCount<int>(2);

        Assert.Equal("Must contain at least 2 items.", checker(Enumerable.Range(0, count).ToArray()));
    }

    // MaxCount

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public void MaxCount_WithAtMostMaximumItems_ReturnsNoError(int count)
    {
        var checker = CollectionCheckers.MaxCount<int>(2);

        Assert.Null(checker(Enumerable.Range(0, count).ToArray()));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(10)]
    public void MaxCount_WithTooManyItems_ReturnsError(int count)
    {
        var checker = CollectionCheckers.MaxCount<int>(2);

        Assert.Equal("Must not contain more than 2 items.", checker(Enumerable.Range(0, count).ToArray()));
    }
}
