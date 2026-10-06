namespace PixieValidation.Tests;

public class ErrorCollectorCardinalityTests
{
    private interface IAlternative;

    private sealed record AlternativeA : IAlternative;

    private sealed record AlternativeB : IAlternative;

    private sealed class AlternativeGroup(IAlternative? first, IAlternative? second) : IValidatable
    {
        public void ValidateAndCollect(ErrorCollector errors) =>
            errors.CheckExactlyOneIsSet(first, second);
    }
    
    [Fact]
    public void CheckExactlyOneIsSet_WithExactlyOneSet_DoesNotAddError()
    {
        var errors = new ErrorCollector();

        errors.CheckExactlyOneIsSet<IAlternative>(null, new AlternativeB());

        Assert.Empty(errors);
    }

    [Fact]
    public void CheckExactlyOneIsSet_WithNoneSet_AddsError()
    {
        var errors = new ErrorCollector();

        errors.CheckExactlyOneIsSet<IAlternative>(null, null);

        var error = Assert.Single(errors);
        Assert.Equal("Exactly one value must be set, but none are set.", error.Message);
    }

    [Fact]
    public void CheckExactlyOneIsSet_WithTwoSet_AddsErrorWithCount()
    {
        var errors = new ErrorCollector();

        errors.CheckExactlyOneIsSet<IAlternative>(new AlternativeA(), new AlternativeB());

        var error = Assert.Single(errors);
        Assert.Equal("Exactly one value must be set, but 2 are set.", error.Message);
    }

    [Fact]
    public void CheckExactlyOneIsSet_InsideNested_UsesPathOfGroupWithoutTrailingDot()
    {
        var errors = new ErrorCollector();
        var group = new AlternativeGroup(null, null);

        errors.Nested(group);

        var error = Assert.Single(errors);
        Assert.Equal("group", error.Path);
    }
}
