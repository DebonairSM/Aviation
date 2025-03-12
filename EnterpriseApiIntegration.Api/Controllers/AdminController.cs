using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseApiIntegration.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Policy = "RequireAdminRole")]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger)
    {
        _logger = logger;
    }

    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        _logger.LogInformation("Admin accessing all users list");
        // Simulate fetching all users
        var users = new[]
        {
            new { Id = 1, Name = "User 1", Role = "InternalUser" },
            new { Id = 2, Name = "User 2", Role = "ExternalPartner" }
        };
        return Ok(users);
    }

    [HttpPost("users/{userId}/role")]
    public IActionResult UpdateUserRole(int userId, [FromBody] string newRole)
    {
        _logger.LogInformation("Admin updating user {UserId} role to {NewRole}", userId, newRole);
        // Simulate updating user role
        return Ok(new { Message = $"User {userId} role updated to {newRole}" });
    }

    [HttpGet("system/settings")]
    public IActionResult GetSystemSettings()
    {
        _logger.LogInformation("Admin accessing system settings");
        // Simulate fetching system settings
        var settings = new { MaxUsers = 100, AllowedDomains = new[] { "company.com" } };
        return Ok(settings);
    }
} 