using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.Domain;

public partial record Club : IValidatable
{
    public static PropChecker<string> NameChecker =
        StringCheckers.NotEmpty();

    public static PropChecker<string> MemberNameChecker =
        StringCheckers.NotEmpty();

    public void ValidateAndCollect(ErrorCollector errors)
    {
        errors.Check(Name, NameChecker);
        errors.CheckAll(MemberNames, MemberNameChecker);
    }
}