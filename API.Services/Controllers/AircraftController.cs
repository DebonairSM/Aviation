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

/// <summary>
/// Controller for managing aircraft information
/// </summary>
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

    /// <summary>
    /// Gets all aircraft in the system
    /// </summary>
    /// <returns>List of all aircraft</returns>
    /// <response code="200">Returns the list of aircraft</response>
    /// <response code="401">If the user is not authenticated</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AircraftDto>), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<IEnumerable<AircraftDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAircraftQuery());
        return Ok(result);
    }

    /// <summary>
    /// Gets a specific aircraft by ID
    /// </summary>
    /// <param name="id">The ID of the aircraft to retrieve</param>
    /// <returns>The requested aircraft</returns>
    /// <response code="200">Returns the requested aircraft</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the aircraft is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AircraftDto), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AircraftDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetAircraftByIdQuery { Id = id });
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Creates a new aircraft
    /// </summary>
    /// <param name="command">The aircraft creation command</param>
    /// <returns>The newly created aircraft</returns>
    /// <response code="201">Returns the newly created aircraft</response>
    /// <response code="400">If the command is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    [HttpPost]
    [ProducesResponseType(typeof(AircraftDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AircraftDto>> Create(CreateAircraftCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing aircraft
    /// </summary>
    /// <param name="id">The ID of the aircraft to update</param>
    /// <param name="command">The aircraft update command</param>
    /// <returns>The updated aircraft</returns>
    /// <response code="200">Returns the updated aircraft</response>
    /// <response code="400">If the command is invalid or IDs don't match</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the aircraft is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AircraftDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AircraftDto>> Update(Guid id, UpdateAircraftCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an aircraft
    /// </summary>
    /// <param name="id">The ID of the aircraft to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the aircraft was successfully deleted</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the aircraft is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAircraftCommand { Id = id });
        return NoContent();
    }
} 