using System.Reflection;
using Enterprise.Application.Interfaces;
using Enterprise.Application.Services;
using Enterprise.Application.Features.Aircraft;
using Enterprise.Application.Features.Aircraft.Queries;
using Enterprise.Application.Features.Aircraft.Commands;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Enterprise.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR handlers
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Register application services
        services.AddScoped<IAircraftService, AircraftService>();

        return services;
    }
}

// Reference class for assembly scanning
public class AssemblyReference { }
