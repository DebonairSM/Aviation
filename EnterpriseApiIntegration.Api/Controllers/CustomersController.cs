using EnterpriseApiIntegration.Application.Customers.Commands.CreateCustomer;
using EnterpriseApiIntegration.Application.Customers.Commands.UpdateCustomer;
using EnterpriseApiIntegration.Application.Customers.Commands.ChangeCustomerRole;
using EnterpriseApiIntegration.Domain.Customers.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseApiIntegration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateCustomerCommand command)
    {
        var customerId = await _mediator.Send(command);
        return Ok(customerId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCustomerCommand command)
    {
        if (id != command.CustomerId)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/role")]
    public async Task<IActionResult> ChangeRole(Guid id, CustomerRole newRole)
    {
        var command = new ChangeCustomerRoleCommand(id, newRole);
        await _mediator.Send(command);
        return NoContent();
    }
} 