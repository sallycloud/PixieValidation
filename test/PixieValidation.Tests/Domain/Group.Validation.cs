using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.Domain;


public partial record Group : IValidatable
{
    public static PropChecker<string> NameChecker =
        StringCheckers.NotEmpty();

    public void ValidateAndCollect(ErrorCollector errors)
    {
        errors.Check(Name, NameChecker);
        errors.Nested(Persons);
    }
}