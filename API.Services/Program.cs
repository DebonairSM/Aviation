using AzureMicroservicesPlatform.Services.Aircraft.Application;
using Enterprise.Domain.Aircraft;
using Enterprise.Infrastructure;
using Enterprise.Infrastructure.Persistence;
using Enterprise.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Reflection;
using MediatR;
using Enterprise.Application.Features.Aircraft.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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

// Register Customer Repository
builder.Services.AddScoped<Enterprise.Domain.Customers.ICustomerRepository, Enterprise.Infrastructure.Persistence.Repositories.CustomerRepository>();

builder.Services.AddInfrastructureServices(builder.Configuration);

// Register Application Services
builder.Services.AddApplicationServices();

// Register MediatR - This is crucial for your query handlers
builder.Services.AddMediatR(cfg => {
    // Register handlers from API.Services assembly
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    
    // Register handlers from Enterprise.Application assembly
    cfg.RegisterServicesFromAssemblyContaining<GetAircraftQuery>();
});

// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["MicrosoftEntraId:Instance"] + builder.Configuration["MicrosoftEntraId:TenantId"];
        options.Audience = builder.Configuration["MicrosoftEntraId:Audience"];
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                    context.Response.StatusCode = 401;
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { error = "Authentication failed" });
                return context.Response.WriteAsync(result);
            }
        };
    });

// Configure Authorization
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Aircraft API", 
        Version = "v1",
        Description = "API for managing aircraft information in the aviation system",
        Contact = new OpenApiContact
        {
            Name = "Aviation Team",
            Email = "support@aviation.com"
        }
    });

    // Add XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    Console.WriteLine($"Looking for XML file at: {xmlPath}");
    Console.WriteLine($"Base directory: {AppContext.BaseDirectory}");
    Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");
    
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
        Console.WriteLine($"Successfully included XML comments from: {xmlPath}");
    }
    else
    {
        Console.WriteLine($"XML file not found at: {xmlPath}");
        // Try to find the file in the current directory and its subdirectories
        var currentDir = Directory.GetCurrentDirectory();
        var files = Directory.GetFiles(currentDir, "*.xml", SearchOption.AllDirectories);
        Console.WriteLine("Found XML files:");
        foreach (var file in files)
        {
            Console.WriteLine($"- {file}");
        }

        // Try to find the file in the bin directory
        var binPath = Path.Combine(currentDir, "bin", "Debug", "net8.0", xmlFile);
        if (File.Exists(binPath))
        {
            c.IncludeXmlComments(binPath);
            Console.WriteLine($"Successfully included XML comments from bin directory: {binPath}");
        }
        else
        {
            Console.WriteLine($"XML file not found in bin directory: {binPath}");
        }
    }

    // Configure JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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

    // Configure response types
    c.MapType<Guid>(() => new OpenApiSchema { Type = "string", Format = "uuid" });
    c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date-time" });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aircraft API V1");
        c.RoutePrefix = "swagger";
    });
}

// Enable routing and endpoints
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
