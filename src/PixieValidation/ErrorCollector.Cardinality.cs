namespace PixieValidation;

public sealed partial class ErrorCollector
{
    /// <summary>
    /// Records an error at the current path unless exactly one of <paramref name="values"/> is set
    /// (not <see langword="null"/>). Meant for groups of alternatives of which exactly one must be chosen.
    /// The error carries no property name, so call this from within the group's own validation,
    /// where the path already points at the group.
    /// </summary>
    public void CheckExactlyOneIsSet<T>(params T?[] values)
        where T : class
    {
        var setCount = values.Count(value => value is not null);

        if (setCount == 1)
            return;

        Check("", setCount == 0
            ? "Exactly one value must be set, but none are set."
            : $"Exactly one value must be set, but {setCount} are set.");
    }
}
