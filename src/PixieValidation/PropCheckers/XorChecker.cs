namespace PixieValidation.PropCheckers;

public readonly struct XorChecker<T>
{
    private readonly PropChecker<T> _checker;
    internal XorChecker(PropChecker<T> checker) => _checker = checker;
    internal PropChecker<T> Checker => _checker;
}