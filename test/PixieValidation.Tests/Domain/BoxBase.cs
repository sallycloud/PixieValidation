namespace PixieValidation.Tests.Domain;

public abstract record BoxBase
{
    public required int Capacity { get; init; }
}