namespace PixieValidation;

public static class PropAgainstCheckerExtensions
{
    /// <summary>
    /// Combines two against-checkers so both must pass. Returns the first error encountered.
    /// </summary>
    public static PropAgainstChecker<T, TAgainst> And<T, TAgainst>(
        this PropAgainstChecker<T, TAgainst> first,
        PropAgainstChecker<T, TAgainst> second) =>
        (toValidate, against) => first(toValidate, against) ?? second(toValidate, against);
}