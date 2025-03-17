using MediatR;
using AzureMicroservicesPlatform.Services.Aircraft.Application.Models;

namespace AzureMicroservicesPlatform.Services.Aircraft.Application.Features.Commands
{
    public record UpdateAircraftCommand : IRequest<AircraftDto>
    {
        public int Id { get; init; }
        public string Registration { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Manufacturer { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public int YearOfManufacture { get; init; }
        public string SerialNumber { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime LastMaintenanceDate { get; init; }
        public DateTime NextMaintenanceDue { get; init; }
        public int TotalFlightHours { get; init; }
    }

    public class UpdateAircraftCommandHandler : IRequestHandler<UpdateAircraftCommand, AircraftDto>
    {
        public async Task<AircraftDto> Handle(UpdateAircraftCommand request, CancellationToken cancellationToken)
        {
            // Demo implementation
            return new AircraftDto
            {
                Id = request.Id,
                Registration = request.Registration,
                Type = request.Type,
                Manufacturer = request.Manufacturer,
                Model = request.Model,
                YearOfManufacture = request.YearOfManufacture,
                SerialNumber = request.SerialNumber,
                Status = request.Status,
                LastMaintenanceDate = request.LastMaintenanceDate,
                NextMaintenanceDue = request.NextMaintenanceDue,
                TotalFlightHours = request.TotalFlightHours
            };
        }
    }
} 