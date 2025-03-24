using System;
using System.Threading;
using System.Threading.Tasks;
using Enterprise.Domain.Aircraft;
using MediatR;
using AzureMicroservicesPlatform.Services.Aircraft.Application.Models;

namespace Enterprise.Application.Features.Aircraft.Commands;

public record UpdateAircraftCommand : IRequest<AircraftDto>
{
    public Guid Id { get; init; }
    public required string Registration { get; init; }
    public required string Type { get; init; }
    public required string Manufacturer { get; init; }
    public required string Model { get; init; }
    public required string SerialNumber { get; init; }
}

public class UpdateAircraftCommandHandler : IRequestHandler<UpdateAircraftCommand, AircraftDto>
{
    private readonly IAircraftRepository _aircraftRepository;

    public UpdateAircraftCommandHandler(IAircraftRepository aircraftRepository)
    {
        _aircraftRepository = aircraftRepository;
    }

    public async Task<AircraftDto> Handle(UpdateAircraftCommand request, CancellationToken cancellationToken)
    {
        var aircraft = await _aircraftRepository.GetByIdAsync(request.Id)
            ?? throw new InvalidOperationException($"Aircraft with ID {request.Id} not found");

        aircraft.Update(
            request.Registration,
            request.Type,
            request.Manufacturer,
            request.Model,
            request.SerialNumber);

        await _aircraftRepository.UpdateAsync(aircraft);

        return new AircraftDto
        {
            Id = aircraft.Id,
            Registration = aircraft.Registration,
            Type = aircraft.Type,
            Manufacturer = aircraft.Manufacturer,
            Model = aircraft.Model,
            SerialNumber = aircraft.SerialNumber,
            Status = aircraft.Status.ToString()
        };
    }
} 