using System;
using System.Threading;
using System.Threading.Tasks;
using Enterprise.Domain.Aircraft;
using MediatR;
using AzureMicroservicesPlatform.Services.Aircraft.Application.Models;

namespace AzureMicroservicesPlatform.Services.Aircraft.Application.Features.Queries;

public record GetAircraftByIdQuery : IRequest<AircraftDto?>
{
    public Guid Id { get; init; }
}

public class GetAircraftByIdQueryHandler : IRequestHandler<GetAircraftByIdQuery, AircraftDto?>
{
    private readonly IAircraftRepository _aircraftRepository;

    public GetAircraftByIdQueryHandler(IAircraftRepository aircraftRepository)
    {
        _aircraftRepository = aircraftRepository;
    }

    public async Task<AircraftDto?> Handle(GetAircraftByIdQuery request, CancellationToken cancellationToken)
    {
        var aircraft = await _aircraftRepository.GetByIdAsync(request.Id);
        if (aircraft is null)
            return null;

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