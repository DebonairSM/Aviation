using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace AzureMicroservicesPlatform.Services.Aircraft.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR handlers from this assembly
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        return services;
    }
} 