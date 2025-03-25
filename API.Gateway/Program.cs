using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add Ocelot configuration explicitly from the file
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}";
        options.Audience = builder.Configuration["AzureAd:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = $"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}/v2.0",
            ValidAudience = builder.Configuration["AzureAd:Audience"]
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"Token validated successfully for user: {context.Principal?.Identity?.Name ?? "unknown"}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure Ocelot
builder.Services.AddOcelot(builder.Configuration);

// Configure Swagger for Ocelot - simplified
builder.Services.AddSwaggerForOcelot(builder.Configuration);

// Add minimal Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerForOcelotUI(opt =>
    {
        opt.PathToSwaggerGenerator = "/swagger/docs";
    });
}

// Add request header logging middleware
app.Use(async (context, next) =>
{
    Console.WriteLine("\n=== Incoming Request ===");
    Console.WriteLine($"Path: {context.Request.Path}");
    Console.WriteLine($"Method: {context.Request.Method}");
    Console.WriteLine("Headers:");
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"  {header.Key}: {header.Value}");
    }
    Console.WriteLine("=====================\n");

    await next();
});

// Add Ocelot configuration debugging
app.Use(async (context, next) =>
{
    var configuration = app.Configuration.GetSection("Routes").Get<object>();
    Console.WriteLine("Ocelot Routes Configuration:");
    Console.WriteLine(configuration != null 
        ? "Routes configuration found" 
        : "No routes configuration found");
    
    await next();
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

Console.WriteLine("Starting Ocelot...");
// Configure Ocelot
await app.UseOcelot();

app.Run();
