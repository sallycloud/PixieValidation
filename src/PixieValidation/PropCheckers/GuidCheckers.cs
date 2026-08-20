namespace PixieValidation.PropCheckers;

public static class GuidCheckers
{
    public static PropChecker<Guid> NotEmpty() =>
        value => value == Guid.Empty ? "Must not be empty." : null;
}