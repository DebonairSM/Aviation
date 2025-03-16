using System.Reflection;
using EnterpriseApiIntegration.Domain.Aircraft;
using EnterpriseApiIntegration.Infrastructure.Persistence;
using EnterpriseApiIntegration.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EnterpriseApiIntegration.Application;
using EnterpriseApiIntegration.Application.Interfaces;

namespace EnterpriseApiIntegration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Register repositories
        services.AddScoped<IAircraftRepository, AircraftRepository>();

        return services;
    }
}
