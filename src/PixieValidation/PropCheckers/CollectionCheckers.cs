namespace PixieValidation.PropCheckers;

public static class CollectionCheckers
{
    public static PropChecker<IReadOnlyCollection<T>> NotEmpty<T>() =>
        value => value.Count == 0 ? "Must not be empty." : null;

    public static PropChecker<IReadOnlyCollection<T>> MinCount<T>(int min) =>
        value => value.Count < min ? $"Must contain at least {min} items." : null;

    public static PropChecker<IReadOnlyCollection<T>> MaxCount<T>(int max) =>
        value => value.Count > max ? $"Must not contain more than {max} items." : null;
    
    /// <summary>
    /// Reduces a per-item <paramref name="itemChecker"/> into a single <see cref="PropChecker{T}"/>
    /// over the whole sequence, for use where only a single <c>PropChecker&lt;T&gt;</c> can be applied
    /// (e.g. inside <see cref="StringCheckers"/>-style composition, or a plain <c>if (checker(value) is
    /// {} error)</c> check without an <see cref="ErrorCollector"/> at hand).
    /// </summary>
    /// <remarks>
    /// Stops at the first invalid element and returns a single combined message including the
    /// offending value. Unlike <see cref="ErrorCollector.CheckAll{T}(IReadOnlyCollection{T}, PropChecker{T}, string)"/>,
    /// it does not report every invalid element with an indexed path — it collapses the whole sequence into one
    /// pass/fail result. Prefer <c>CheckAll</c> whenever an <see cref="ErrorCollector"/> is available
    /// and you want every invalid element reported individually.
    /// </remarks>
    public static PropChecker<IEnumerable<T>> FirstInvalid<T>(PropChecker<T> itemChecker) =>
        values =>
        {
            foreach (var value in values)
            {
                var error = itemChecker(value);
                if (error is not null)
                    return $"{error} ({value})";
            }
            return null;
        };
}