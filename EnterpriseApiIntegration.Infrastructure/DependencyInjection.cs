using System.Reflection;
using EnterpriseApiIntegration.Domain.Customers;
using EnterpriseApiIntegration.Infrastructure.Persistence;
using EnterpriseApiIntegration.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EnterpriseApiIntegration.Application;

namespace EnterpriseApiIntegration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register Write DbContext
        services.AddDbContext<WriteDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("WriteDatabase"),
                b => b.MigrationsAssembly(typeof(WriteDbContext).Assembly.FullName)));

        // Register Read DbContext
        services.AddDbContext<ReadDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ReadDatabase"),
                b => b.MigrationsAssembly(typeof(ReadDbContext).Assembly.FullName)));

        // Register repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // Register MediatR
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            // Also register handlers from Application layer
            cfg.RegisterServicesFromAssembly(typeof(EnterpriseApiIntegration.Application.AssemblyReference).Assembly);
        });

        return services;
    }
}
