using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseApiIntegration.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Policy = "RequireExternalPartnerRole")]
public class PartnerController : ControllerBase
{
    private readonly ILogger<PartnerController> _logger;

    public PartnerController(ILogger<PartnerController> logger)
    {
        _logger = logger;
    }

    [HttpGet("products")]
    public IActionResult GetProducts()
    {
        _logger.LogInformation("Partner accessing product catalog");
        // Simulate fetching products
        var products = new[]
        {
            new { Id = 1, Name = "Product A", Price = 100 },
            new { Id = 2, Name = "Product B", Price = 200 }
        };
        return Ok(products);
    }

    [HttpPost("orders")]
    public IActionResult CreateOrder([FromBody] object order)
    {
        _logger.LogInformation("Partner creating new order");
        // Simulate creating a new order
        return CreatedAtAction(nameof(GetOrderStatus), new { orderId = 123 }, null);
    }

    [HttpGet("orders/{orderId}/status")]
    public IActionResult GetOrderStatus(int orderId)
    {
        _logger.LogInformation("Partner checking order status for {OrderId}", orderId);
        // Simulate fetching order status
        var status = new { OrderId = orderId, Status = "Processing", EstimatedDelivery = DateTime.UtcNow.AddDays(5) };
        return Ok(status);
    }

    [HttpGet("account/profile")]
    public IActionResult GetPartnerProfile()
    {
        _logger.LogInformation("Partner accessing their profile");
        // Simulate fetching partner profile
        var profile = new { CompanyName = "Partner Corp", MemberSince = "2024-01-01", Tier = "Gold" };
        return Ok(profile);
    }
} 