using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseApiIntegration.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Policy = "RequireInternalUserRole")]
public class InternalController : ControllerBase
{
    private readonly ILogger<InternalController> _logger;

    public InternalController(ILogger<InternalController> logger)
    {
        _logger = logger;
    }

    [HttpGet("documents")]
    public IActionResult GetInternalDocuments()
    {
        _logger.LogInformation("Internal user accessing documents");
        // Simulate fetching internal documents
        var documents = new[]
        {
            new { Id = 1, Title = "Internal Policy", Type = "Confidential" },
            new { Id = 2, Title = "Employee Handbook", Type = "Internal" }
        };
        return Ok(documents);
    }

    [HttpPost("documents")]
    public IActionResult CreateInternalDocument([FromBody] object document)
    {
        _logger.LogInformation("Internal user creating new document");
        // Simulate creating a new internal document
        return CreatedAtAction(nameof(GetInternalDocuments), new { Id = 3 });
    }

    [HttpGet("reports/analytics")]
    public IActionResult GetAnalytics()
    {
        _logger.LogInformation("Internal user accessing analytics");
        // Simulate fetching analytics data
        var analytics = new { TotalDocuments = 100, ActiveUsers = 50 };
        return Ok(analytics);
    }
} 