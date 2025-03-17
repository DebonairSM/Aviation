using System.Reflection;
using EnterpriseApiIntegration.Application.Interfaces;
using EnterpriseApiIntegration.Application.Services;
using EnterpriseApiIntegration.Application.Features.Aircraft;
using EnterpriseApiIntegration.Application.Features.Customers;
using EnterpriseApiIntegration.Application.Features.Subscriptions;
using EnterpriseApiIntegration.Application.Customers.Commands.CreateCustomer;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseApiIntegration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<IAircraftService, AircraftService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();

        return services;
    }

    public static IServiceCollection AddAircraftServices(this IServiceCollection services)
    {
        // Register MediatR handlers for Aircraft service
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssemblyContaining<AircraftDto>();
        });

        // Register Aircraft-specific services
        services.AddScoped<IAircraftService, AircraftService>();

        return services;
    }

    public static IServiceCollection AddCustomerServices(this IServiceCollection services)
    {
        // Register MediatR handlers for Customer service
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(CustomerDto).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
        });

        // Register Customer-specific services
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }

    public static IServiceCollection AddSubscriptionServices(this IServiceCollection services)
    {
        // Register MediatR handlers for Subscription service
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssemblyContaining<SubscriptionDto>();
        });

        // Register Subscription-specific services
        services.AddScoped<ISubscriptionService, SubscriptionService>();

        return services;
    }
}

// Reference class for assembly scanning
public class AssemblyReference { }
