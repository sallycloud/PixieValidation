namespace PixieValidation.Tests.Domain;

public partial record Club
{
    public required string Name { get; init; }
    public required IReadOnlyList<string> MemberNames { get; init; }
}