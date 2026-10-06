namespace PixieValidation.Tests;

public class ErrorCollectorPathTests
{
    [Fact]
    public void Check_WithEmptyPropertyNameAtRoot_UsesEmptyPath()
    {
        var errors = new ErrorCollector();

        errors.Check("", "Some error.");

        var error = Assert.Single(errors);
        Assert.Equal("", error.Path);
    }

    [Fact]
    public void Check_WithEmptyPropertyNameInsideNested_UsesPathWithoutTrailingDot()
    {
        var errors = new ErrorCollector();
        var child = new ChildReportingWithEmptyName();

        errors.Nested(child);

        var error = Assert.Single(errors);
        Assert.Equal("child", error.Path);
    }

    [Fact]
    public void Check_WithEmptyPropertyNameInsideDeeplyNested_UsesFullPrefixWithoutTrailingDot()
    {
        var errors = new ErrorCollector();
        var outer = new OuterWrappingChild(new ChildReportingWithEmptyName());

        errors.Nested(outer);

        var error = Assert.Single(errors);
        Assert.Equal("outer.Inner", error.Path);
    }

    private sealed class ChildReportingWithEmptyName : IValidatable
    {
        public void ValidateAndCollect(ErrorCollector errors) =>
            errors.Check("", "Some error.");
    }

    private sealed class OuterWrappingChild(ChildReportingWithEmptyName inner) : IValidatable
    {
        public ChildReportingWithEmptyName Inner { get; } = inner;

        public void ValidateAndCollect(ErrorCollector errors) =>
            errors.Nested(Inner);
    }
}