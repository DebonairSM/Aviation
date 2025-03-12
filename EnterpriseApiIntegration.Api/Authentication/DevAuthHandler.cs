using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace EnterpriseApiIntegration.Api.Authentication;

public class DevAuthHandler : AuthenticationHandler<DevAuthOptions>
{
    private readonly DevAuthOptions _options;

    public DevAuthHandler(
        IOptionsMonitor<DevAuthOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        TimeProvider timeProvider)
        : base(options, logger, encoder, new TimeProviderClock(timeProvider))
    {
        _options = options.CurrentValue;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // For development, we'll use the first user in the list as the default
        var devUser = _options.Users.FirstOrDefault() ?? new DevUser 
        { 
            Id = "default-dev-user",
            Name = "Default Dev User",
            Email = "default@dev.local",
            Roles = new List<string> { "Admin" }
        };

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, devUser.Id),
            new Claim(ClaimTypes.Name, devUser.Name),
            new Claim(ClaimTypes.Email, devUser.Email)
        };

        // Add role claims
        claims.AddRange(devUser.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, "DevAuth");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
} 