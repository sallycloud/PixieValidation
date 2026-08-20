namespace PixieValidation;

/// <summary>
/// Checks a single value against a rule.
/// </summary>
/// <returns><see langword="null"/> if the value is valid; otherwise, the error message.</returns>
public delegate string? PropChecker<in T>(T toValidate);