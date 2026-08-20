namespace PixieValidation.Tests.Domain;

public partial record Group
{
    public required string Name { get; init; }
    public required IReadOnlyList<Person> Persons { get; init; }
}