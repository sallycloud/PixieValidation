namespace PixieValidation.PropCheckers;

public static class CollectionCheckers
{
    public static PropChecker<IReadOnlyCollection<T>> NotEmpty<T>() =>
        value => value.Count == 0 ? "Must not be empty." : null;

    public static PropChecker<IReadOnlyCollection<T>> MinCount<T>(int min) =>
        value => value.Count < min ? $"Must contain at least {min} items." : null;

    public static PropChecker<IReadOnlyCollection<T>> MaxCount<T>(int max) =>
        value => value.Count > max ? $"Must not contain more than {max} items." : null;
}