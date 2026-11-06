using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class GuidCheckersTests
{
    [Fact]
    public void NotEmpty_WithEmptyGuid_ReturnsError()
    {
        var checker = GuidCheckers.NotEmpty();

        Assert.Equal("Must not be empty.", checker(Guid.Empty));
    }

    [Fact]
    public void NotEmpty_WithNonEmptyGuid_ReturnsNoError()
    {
        var checker = GuidCheckers.NotEmpty();

        Assert.Null(checker(Guid.NewGuid()));
    }
}
