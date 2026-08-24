namespace PixieValidation.PropCheckers;

public readonly struct NotChecker<T>
{
    private readonly PropChecker<T> _checker;
    internal NotChecker(PropChecker<T> checker) => _checker = checker;
    internal PropChecker<T> Checker => _checker;
}