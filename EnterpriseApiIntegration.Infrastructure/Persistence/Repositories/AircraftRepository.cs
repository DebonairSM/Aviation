using EnterpriseApiIntegration.Domain.Aircraft;

namespace EnterpriseApiIntegration.Infrastructure.Persistence.Repositories;

public class AircraftRepository : IAircraftRepository
{
    private readonly ApplicationDbContext _context;

    public AircraftRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Aircraft> GetByIdAsync(int id)
    {
        // Demo implementation
        return new Aircraft
        {
            Id = id,
            Registration = "N12345",
            Type = "Commercial",
            Manufacturer = "Boeing",
            Model = "737-800",
            YearOfManufacture = 2020,
            SerialNumber = "12345",
            Status = "Active",
            LastMaintenanceDate = DateTime.Now.AddMonths(-3),
            NextMaintenanceDue = DateTime.Now.AddMonths(3),
            TotalFlightHours = 5000
        };
    }

    public async Task<IEnumerable<Aircraft>> GetAllAsync()
    {
        // Demo implementation
        return new List<Aircraft>
        {
            new Aircraft
            {
                Id = 1,
                Registration = "N12345",
                Type = "Commercial",
                Manufacturer = "Boeing",
                Model = "737-800",
                YearOfManufacture = 2020,
                SerialNumber = "12345",
                Status = "Active",
                LastMaintenanceDate = DateTime.Now.AddMonths(-3),
                NextMaintenanceDue = DateTime.Now.AddMonths(3),
                TotalFlightHours = 5000
            },
            new Aircraft
            {
                Id = 2,
                Registration = "N54321",
                Type = "Commercial",
                Manufacturer = "Airbus",
                Model = "A320",
                YearOfManufacture = 2019,
                SerialNumber = "54321",
                Status = "Active",
                LastMaintenanceDate = DateTime.Now.AddMonths(-2),
                NextMaintenanceDue = DateTime.Now.AddMonths(4),
                TotalFlightHours = 4000
            }
        };
    }

    public async Task<Aircraft> AddAsync(Aircraft aircraft)
    {
        // Demo implementation
        aircraft.Id = 3;
        return aircraft;
    }

    public async Task<Aircraft> UpdateAsync(Aircraft aircraft)
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