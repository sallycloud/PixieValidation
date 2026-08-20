using PixieValidation.Tests.Domain;

namespace PixieValidation.Domain;

public class PersonValidator : IValidator<Person>
{
    public void ValidateAndCollect(Person toValidate, ErrorCollector errors)
    {
        errors.Check(toValidate.Name, Person.NameChecker, nameof(Person.Name));
        errors.Check(toValidate.Age, Person.AgeChecker, nameof(Person.Age));
    }
}