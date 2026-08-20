namespace PixieValidation.Tests.Domain;

public partial record Person
{
    public required string Name { get; init; }
    public required int Age { get; init; }
}