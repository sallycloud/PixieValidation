namespace PixieValidation;

internal static class CollectionValidationHelper
{
    public static IEnumerable<ValidationError> CollectErrors<T>(
        IReadOnlyCollection<T> values,
        PropChecker<T> checker,
        string basePath)
    {
        var i = 0;
        foreach (var value in values)
        {
            var error = checker(value);
            if (error is not null)
                yield return new ValidationError($"{basePath}[{i}]", error);
            i++;
        }
    }
}