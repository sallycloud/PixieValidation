namespace PixieValidation.PropCheckers;

public static class EnumFlagsCheckers
{
    public static PropChecker<T> IsValidCombination<T>() where T : struct, Enum =>
        value =>
        {
            var allFlags = GetAllFlags<T>();
            return !value.Equals(Enum.ToObject(typeof(T), Convert.ToInt64(value) & Convert.ToInt64(allFlags)))
                ? "Contains an undefined flag."
                : null;
        };

    private static T GetAllFlags<T>() where T : struct, Enum
    {
        long combined = 0;
        foreach (var flag in Enum.GetValues<T>())
            combined |= Convert.ToInt64(flag);
        return (T)Enum.ToObject(typeof(T), combined);
    }
}