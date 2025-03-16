using MediatR;

namespace EnterpriseApiIntegration.Application.Features.Aircraft.Queries;

public record GetAircraftByIdQuery : IRequest<AircraftDto>
{
    public int Id { get; init; }
}

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