using System.Reflection;
using EnterpriseApiIntegration.Application.Interfaces;
using EnterpriseApiIntegration.Application.Services;
using EnterpriseApiIntegration.Application.Features.Aircraft;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseApiIntegration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR services
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(AircraftDto).Assembly);
        });

        // Register application services
        services.AddScoped<IExampleService, ExampleService>();
        services.AddScoped<IAircraftService, AircraftService>();

        return services;
    }
}

// Reference class for assembly scanning
public class AssemblyReference { }
