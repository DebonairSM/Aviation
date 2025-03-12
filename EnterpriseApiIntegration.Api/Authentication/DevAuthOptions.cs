using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace EnterpriseApiIntegration.Api.Authentication;

public class DevAuthOptions : AuthenticationSchemeOptions
{
    public List<DevUser> Users { get; set; } = new();
}

public class DevUser
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
} 