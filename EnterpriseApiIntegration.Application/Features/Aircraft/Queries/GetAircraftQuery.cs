using MediatR;

namespace EnterpriseApiIntegration.Application.Features.Aircraft.Queries;

public record GetAircraftQuery : IRequest<IEnumerable<AircraftDto>>;

public class GetAircraftQueryHandler : IRequestHandler<GetAircraftQuery, IEnumerable<AircraftDto>>
{
    public async Task<IEnumerable<AircraftDto>> Handle(GetAircraftQuery request, CancellationToken cancellationToken)
    {
        // Demo implementation
        return new List<AircraftDto>
        {
            new()
            {
                Id = 1,
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
            }
        };
    }
} 