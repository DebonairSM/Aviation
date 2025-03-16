using EnterpriseApiIntegration.Application.Features.Aircraft;
using EnterpriseApiIntegration.Application.Interfaces;
using EnterpriseApiIntegration.Domain.Aircraft;

namespace EnterpriseApiIntegration.Application.Services;

public class AircraftService : IAircraftService
{
    private readonly IAircraftRepository _aircraftRepository;

    public AircraftService(IAircraftRepository aircraftRepository)
    {
        _aircraftRepository = aircraftRepository;
    }

    public async Task<AircraftDto> GetByIdAsync(int id)
    {
        // Demo implementation
        return new AircraftDto
        {
            Id = id,
            Model = "Boeing 737",
            Manufacturer = "Boeing",
            YearOfManufacture = 2020,
            Registration = "N12345",
            Type = "Commercial",
            SerialNumber = "12345",
            Status = "Active",
            LastMaintenanceDate = DateTime.Now.AddMonths(-3),
            NextMaintenanceDue = DateTime.Now.AddMonths(3),
            TotalFlightHours = 5000
        };
    }

    public async Task<IEnumerable<AircraftDto>> GetAllAsync()
    {
        // Demo implementation
        return new List<AircraftDto>
        {
            new AircraftDto
            {
                Id = 1,
                Model = "Boeing 737",
                Manufacturer = "Boeing",
                YearOfManufacture = 2020,
                Registration = "N12345",
                Type = "Commercial",
                SerialNumber = "12345",
                Status = "Active",
                LastMaintenanceDate = DateTime.Now.AddMonths(-3),
                NextMaintenanceDue = DateTime.Now.AddMonths(3),
                TotalFlightHours = 5000
            },
            new AircraftDto
            {
                Id = 2,
                Model = "Airbus A320",
                Manufacturer = "Airbus",
                YearOfManufacture = 2019,
                Registration = "N54321",
                Type = "Commercial",
                SerialNumber = "54321",
                Status = "Active",
                LastMaintenanceDate = DateTime.Now.AddMonths(-2),
                NextMaintenanceDue = DateTime.Now.AddMonths(4),
                TotalFlightHours = 4000
            }
        };
    }

    public async Task<AircraftDto> CreateAsync(AircraftDto aircraft)
    {
        // Demo implementation
        aircraft.Id = 3;
        return aircraft;
    }

    public async Task<AircraftDto> UpdateAsync(AircraftDto aircraft)
    {
        // Demo implementation
        return aircraft;
    }

    public async Task DeleteAsync(int id)
    {
        // Demo implementation
        await Task.CompletedTask;
    }
} 