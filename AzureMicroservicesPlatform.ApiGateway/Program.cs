using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Configure URLs
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // HTTP on 8080
    serverOptions.ListenAnyIP(8080);
    // HTTPS on 7080 (if you have a certificate configured)
    // serverOptions.ListenAnyIP(7080, configure => configure.UseHttps());
});

// Add services to the container.
builder.Services.AddControllers();

// Configure Ocelot
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddJsonFile("ocelot.json")
    .AddEnvironmentVariables();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });
});

// Configure Swagger for Ocelot
builder.Services.AddSwaggerForOcelot(builder.Configuration);

// Configure Ocelot
builder.Services.AddOcelot(builder.Configuration);

// Configure Azure AD Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("MicrosoftEntraId"));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Always enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway V1");
    c.RoutePrefix = "swagger"; // This makes /swagger work directly
});
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Map controllers before Ocelot
app.MapControllers();

// Use Ocelot last in the pipeline
await app.UseOcelot();

app.Run();