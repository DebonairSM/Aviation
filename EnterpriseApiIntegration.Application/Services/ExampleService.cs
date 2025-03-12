using EnterpriseApiIntegration.Application.Interfaces;

namespace EnterpriseApiIntegration.Application.Services;

public class ExampleService : IExampleService
{
    public object GetExampleEntity()
    {
        return new { Message = "Example entity from service" };
    }
}
