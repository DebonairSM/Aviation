using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using EnterpriseApiIntegration.Infrastructure.Persistence;
using EnterpriseApiIntegration.Domain.Aircraft;
using EnterpriseApiIntegration.Infrastructure.Persistence.Repositories;
using EnterpriseApiIntegration.Infrastructure;
using MediatR;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Newtonsoft;

var builder = WebApplication.CreateBuilder(args);

// Configure URLs and Ports
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    var port = builder.Configuration.GetValue<int>("Port", 5002);
    serverOptions.ListenAnyIP(port);
});

// Add services to the container
builder.Services.AddControllers();

// Configure Database
var connectionString = builder.Environment.IsDevelopment() 
    ? builder.Configuration.GetConnectionString("DefaultConnection")
    : builder.Configuration.GetConnectionString("DockerConnection");

// Register DbContexts
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

builder.Services.AddDbContext<WriteDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

// Register Infrastructure Services
builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Register MediatR and handlers
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ApplicationDbContext).Assembly);
});

// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("MicrosoftEntraId"));

// Configure Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireClaim("roles", "Admin"));
    options.AddPolicy("RequireInternalUserRole", policy =>
        policy.RequireClaim("roles", "Admin", "InternalUser"));
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Aircraft Service API",
        Version = "v1",
        Description = "API for managing aircraft-related operations and customer interactions",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@aircraftservice.com"
        }
    });

    // Configure Swagger to use JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Enable annotations for more detailed API documentation
    c.EnableAnnotations();

    // Use fully qualified schema names to prevent conflicts
    c.CustomSchemaIds(type => type.FullName);
});

// Configure Swagger middleware with additional options
builder.Services.AddSwaggerGenNewtonsoftSupport(); // For better JSON handling

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.SerializeAsV2 = false; // Use OpenAPI 3.0
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aircraft Service API v1");
        c.RoutePrefix = "swagger";
        c.DocExpansion(DocExpansion.List);
        c.DefaultModelsExpandDepth(2);
        c.EnableDeepLinking();
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

// Add authentication & authorization to the request pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }
