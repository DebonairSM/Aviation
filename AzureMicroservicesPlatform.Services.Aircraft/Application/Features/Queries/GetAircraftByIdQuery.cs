using MediatR;
using AzureMicroservicesPlatform.Services.Aircraft.Application.Models;

namespace AzureMicroservicesPlatform.Services.Aircraft.Application.Features.Queries;

public record GetAircraftByIdQuery(int Id) : IRequest<AircraftDto>;

public class GetAircraftByIdQueryHandler : IRequestHandler<GetAircraftByIdQuery, AircraftDto>
{
    public async Task<AircraftDto> Handle(GetAircraftByIdQuery request, CancellationToken cancellationToken)
    {
        // Demo implementation
        return new AircraftDto
        {
            Id = request.Id,
            Registration = "N12345",
            Type = "Commercial",
            Manufacturer = "Boeing",
            Model = "737-800",
            YearOfManufacture = 2015,
            SerialNumber = "12345",
            Status = "Active",
            LastMaintenanceDate = DateTime.UtcNow.AddMonths(-1),
            NextMaintenanceDue = DateTime.UtcNow.AddMonths(5),
            TotalFlightHours = 15000
        };
    }
} 