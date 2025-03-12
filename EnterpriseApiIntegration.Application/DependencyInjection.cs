using EnterpriseApiIntegration.Application.Interfaces;
using EnterpriseApiIntegration.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseApiIntegration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IExampleService, ExampleService>();
        return services;
    }
}
