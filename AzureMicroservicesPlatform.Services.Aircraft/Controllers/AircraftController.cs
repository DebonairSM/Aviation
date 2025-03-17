using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AzureMicroservicesPlatform.Services.Aircraft.Application.Features.Commands;
using AzureMicroservicesPlatform.Services.Aircraft.Application.Features.Queries;

namespace AzureMicroservicesPlatform.Services.Aircraft.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AircraftController : ControllerBase
{
    private readonly IMediator _mediator;

    public AircraftController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAircraft()
    {
        var query = new GetAircraftQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAircraft(int id)
    {
        var query = new GetAircraftByIdQuery(id);
        var result = await _mediator.Send(query);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAircraft([FromBody] CreateAircraftCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAircraft), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAircraft(int id, [FromBody] UpdateAircraftCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAircraft(int id)
    {
        var command = new DeleteAircraftCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
} 