using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class ByteArrayCheckersTests
{
    [Fact]
    public void NotEmpty_WithEmptyArray_ReturnsError() =>
        Assert.Equal("Must not be empty.", ByteArrayCheckers.NotEmpty()([]));

    [Fact]
    public void NotEmpty_WithOneByte_ReturnsNoError() =>
        Assert.Null(ByteArrayCheckers.NotEmpty()(new byte[1]));

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    public void MinLength_AtOrAboveMinimum_ReturnsNoError(int length) =>
        Assert.Null(ByteArrayCheckers.MinLength(3)(new byte[length]));

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public void MinLength_BelowMinimum_ReturnsError(int length) =>
        Assert.Equal("Must be at least 3 bytes.", ByteArrayCheckers.MinLength(3)(new byte[length]));

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    public void MaxLength_AtOrBelowMaximum_ReturnsNoError(int length) =>
        Assert.Null(ByteArrayCheckers.MaxLength(3)(new byte[length]));

    [Theory]
    [InlineData(4)]
    [InlineData(100)]
    public void MaxLength_AboveMaximum_ReturnsError(int length) =>
        Assert.Equal("Must not exceed 3 bytes.", ByteArrayCheckers.MaxLength(3)(new byte[length]));
}
