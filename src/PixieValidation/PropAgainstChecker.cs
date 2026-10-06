namespace PixieValidation;

/// <summary>
/// Checks a single value against a rule that additionally depends on <typeparamref name="TAgainst"/>.
/// </summary>
/// <returns><see langword="null"/> if the value is valid; otherwise, the error message.</returns>
public delegate string? PropAgainstChecker<in T, in TAgainst>(T toValidate, TAgainst against);