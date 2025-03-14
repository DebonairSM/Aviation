using System.Reflection;
using EnterpriseApiIntegration.Application.Interfaces;
using EnterpriseApiIntegration.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseApiIntegration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR handlers from this assembly
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IExampleService, ExampleService>();
        return services;
    }
}

// Reference class for assembly scanning
public class AssemblyReference { }
