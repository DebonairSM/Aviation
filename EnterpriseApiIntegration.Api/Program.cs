using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using EnterpriseApiIntegration.Api.Authentication;
using EnterpriseApiIntegration.Application;
using EnterpriseApiIntegration.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add TimeProvider
builder.Services.AddSingleton(TimeProvider.System);

// Configure authentication based on environment
if (builder.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("UseDevAuthentication"))
{
    builder.Services.Configure<DevAuthOptions>(builder.Configuration.GetSection("DevAuth"));
    builder.Services.AddAuthentication("DevAuth")
        .AddScheme<DevAuthOptions, DevAuthHandler>("DevAuth", options => 
        {
            builder.Configuration.GetSection("DevAuth").Bind(options);
        });
}
else
{
    // Add Microsoft Entra ID Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("MicrosoftEntraId"))
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddInMemoryTokenCaches();
}

// Add Authorization with policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("RequireInternalUserRole", policy =>
        policy.RequireRole("InternalUser"));
    options.AddPolicy("RequireExternalPartnerRole", policy =>
        policy.RequireRole("ExternalPartner"));
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

// Configure Swagger with OAuth security
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Enterprise API Integration", Version = "v1" });
    
    if (!builder.Environment.IsDevelopment() || !builder.Configuration.GetValue<bool>("UseDevAuthentication"))
    {
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                Implicit = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"{builder.Configuration["MicrosoftEntraId:Instance"]}{builder.Configuration["MicrosoftEntraId:TenantId"]}/oauth2/v2.0/authorize"),
                    TokenUrl = new Uri($"{builder.Configuration["MicrosoftEntraId:Instance"]}{builder.Configuration["MicrosoftEntraId:TenantId"]}/oauth2/v2.0/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        { $"api://{builder.Configuration["MicrosoftEntraId:ClientId"]}/access_as_user", "Access API as user" }
                    }
                }
            }
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                },
                new[] { $"api://{builder.Configuration["MicrosoftEntraId:ClientId"]}/access_as_user" }
            }
        });
    }
});

// Register application and infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Enterprise API Integration v1");
        c.OAuthClientId(builder.Configuration["MicrosoftEntraId:ClientId"]);
        c.OAuthScopes($"api://{builder.Configuration["MicrosoftEntraId:ClientId"]}/access_as_user");
    });
}

app.UseHttpsRedirection();

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

// Map health check endpoint
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
