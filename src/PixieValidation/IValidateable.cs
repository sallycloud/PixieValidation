namespace PixieValidation;

public interface IValidatable
{
    void Validate(ErrorCollector errors);
}