namespace PixieValidation.PropCheckers;

public readonly struct OrChecker<T>
{
    private readonly PropChecker<T> _checker;
    internal OrChecker(PropChecker<T> checker) => _checker = checker;
    internal PropChecker<T> Checker => _checker;
}