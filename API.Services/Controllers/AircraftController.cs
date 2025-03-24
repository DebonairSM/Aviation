using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enterprise.Application.Features.Aircraft;
using Enterprise.Application.Features.Aircraft.Commands;
using Enterprise.Application.Features.Aircraft.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<IEnumerable<AircraftDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAircraftQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AircraftDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetAircraftByIdQuery { Id = id });
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AircraftDto>> Create(CreateAircraftCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AircraftDto>> Update(Guid id, UpdateAircraftCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAircraftCommand { Id = id });
        return NoContent();
    }
} 