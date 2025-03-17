using Microsoft.Extensions.DependencyInjection;
using EnterpriseApiIntegration.Application;
using EnterpriseApiIntegration.Infrastructure;
using EnterpriseApiIntegration.Domain.Customers;
using EnterpriseApiIntegration.Infrastructure.Persistence;
using EnterpriseApiIntegration.Infrastructure.Persistence.Repositories;
using MediatR;

namespace AzureMicroservicesPlatform.Services.Customers;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomerServices(this IServiceCollection services)
    {
        // Register Application Services
        services.AddScoped<IMediator, Mediator>();
        
        // Register Infrastructure Services
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        
        // Register MediatR handlers
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        return services;
    }
} 