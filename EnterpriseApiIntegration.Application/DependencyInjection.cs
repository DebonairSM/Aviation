using System.Reflection;
using EnterpriseApiIntegration.Application.Interfaces;
using EnterpriseApiIntegration.Application.Services;
using EnterpriseApiIntegration.Application.Features.Aircraft;
using EnterpriseApiIntegration.Application.Features.Aircraft.Queries;
using EnterpriseApiIntegration.Application.Features.Aircraft.Commands;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace EnterpriseApiIntegration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAircraftServices(this IServiceCollection services)
    {
        // Register MediatR handlers for Aircraft service
        services.AddMediatR(cfg => {
            // Register only Aircraft-specific handlers by scanning the specific namespace
            var assembly = typeof(GetAircraftQuery).Assembly;
            var types = assembly.GetTypes()
                .Where(t => t.Namespace?.StartsWith("EnterpriseApiIntegration.Application.Features.Aircraft") == true)
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && 
                             (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                              i.GetGenericTypeDefinition() == typeof(IRequestHandler<>))));
            
            foreach (var type in types)
            {
                var handlerInterfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && 
                               (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                                i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)));

                foreach (var handlerInterface in handlerInterfaces)
                {
                    services.AddTransient(handlerInterface, type);
                }
            }
        });

        // Register Aircraft-specific services
        services.AddScoped<IAircraftService, AircraftService>();

        return services;
    }
}

// Reference class for assembly scanning
public class AssemblyReference { }
