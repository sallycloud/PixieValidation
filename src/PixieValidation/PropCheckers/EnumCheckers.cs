namespace PixieValidation.PropCheckers;

public static class EnumCheckers
{
    public static PropChecker<T> IsDefined<T>() where T : struct, Enum =>
        value => !Enum.IsDefined(value) ? "Is not a valid value." : null;
}