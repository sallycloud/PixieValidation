using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class EnumFlagsCheckersTests
{
    [Flags]
    public enum Permissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4
    }

    [Theory]
    [InlineData(Permissions.None)]
    [InlineData(Permissions.Read)]
    [InlineData(Permissions.Read | Permissions.Write)]
    [InlineData(Permissions.Read | Permissions.Write | Permissions.Execute)]
    public void IsValidCombination_WithDefinedFlags_ReturnsNoError(Permissions value) =>
        Assert.Null(EnumFlagsCheckers.IsValidCombination<Permissions>()(value));

    [Theory]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(255)]
    public void IsValidCombination_WithUndefinedFlag_ReturnsError(int raw) =>
        Assert.Equal("Contains an undefined flag.", EnumFlagsCheckers.IsValidCombination<Permissions>()((Permissions)raw));
}
