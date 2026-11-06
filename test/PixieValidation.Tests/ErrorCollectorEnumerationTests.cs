namespace PixieValidation.Tests;

public class ErrorCollectorEnumerationTests
{
    [Fact]
    public void GetEnumerator_AsNonGenericIEnumerable_EnumeratesTheCollectedErrors()
    {
        var errors = new ErrorCollector();
        errors.Check("Name", "Value is required.");

        System.Collections.IEnumerable nonGeneric = errors;
        var items = nonGeneric.Cast<ValidationError>().ToList();

        var error = Assert.Single(items);
        Assert.Equal("Name", error.Path);
        Assert.Equal("Value is required.", error.Message);
    }
}
