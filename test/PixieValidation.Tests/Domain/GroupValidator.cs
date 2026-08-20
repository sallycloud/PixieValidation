using PixieValidation.Domain;

namespace PixieValidation.Tests.Domain;

public class GroupValidator : IValidator<Group>
{
    private static readonly PersonValidator PersonValidator = new();
    
    public void ValidateAndCollect(Group toValidate, ErrorCollector errors)
    {
        errors.Check(toValidate.Name, Group.NameChecker, nameof(Group.Name));
        errors.Nested(PersonValidator, toValidate.Persons, nameof(Group.Persons));
    }
}