using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AzureMicroservicesPlatform.BackgroundTasks.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Register services
        services.AddLogging();
        services.AddScoped<ISubscriptionService, SubscriptionService>();

        // Add any additional services here
        // Example:
        // services.AddScoped<IEmailService, EmailService>();
        // services.AddScoped<IBillingService, BillingService>();
        // services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
    })
    .Build();

await host.RunAsync(); 