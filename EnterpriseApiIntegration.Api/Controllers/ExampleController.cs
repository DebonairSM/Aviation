using EnterpriseApiIntegration.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseApiIntegration.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class ExampleController : ControllerBase
    {
        private readonly IExampleService _exampleService;

        public ExampleController(IExampleService exampleService)
        {
            _exampleService = exampleService;
        }

        [HttpGet]
        [Authorize(Policy = "RequireInternalUserRole")] // Only internal users can access
        public IActionResult Get()
        {
            var exampleEntity = _exampleService.GetExampleEntity();
            return Ok(exampleEntity);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")] // Only admins can create
        public IActionResult Create([FromBody] object request)
        {
            // Implementation
            return Ok();
        }

        [HttpGet("public")]
        [Authorize(Policy = "RequireExternalPartnerRole")] // External partners can access
        public IActionResult GetPublicData()
        {
            // Implementation for external partners
            return Ok(new { message = "Public data for external partners" });
        }

        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")] // Alternative way to require admin role
        public IActionResult GetAdminData()
        {
            // Implementation for admins only
            return Ok(new { message = "Admin only data" });
        }
    }
}
