using System;
using System.Threading;
using System.Threading.Tasks;
using Enterprise.Application.Features.Aircraft;
using Enterprise.Domain.Aircraft;
using MediatR;

namespace Enterprise.Application.Features.Aircraft.Commands;

public record CreateAircraftCommand : IRequest<AircraftDto>
{
    public string Registration { get; init; }
    public string Type { get; init; }
    public string Manufacturer { get; init; }
    public string Model { get; init; }
    public string SerialNumber { get; init; }
}

public class CreateAircraftCommandHandler : IRequestHandler<CreateAircraftCommand, AircraftDto>
{
    private readonly IAircraftRepository _aircraftRepository;

    public CreateAircraftCommandHandler(IAircraftRepository aircraftRepository)
    {
        _aircraftRepository = aircraftRepository;
    }

    public async Task<AircraftDto> Handle(CreateAircraftCommand request, CancellationToken cancellationToken)
    {
        var aircraft = new Domain.Aircraft.Aircraft(
            request.Registration,
            request.Type,
            request.Manufacturer,
            request.Model,
            request.SerialNumber);

        await _aircraftRepository.AddAsync(aircraft);

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