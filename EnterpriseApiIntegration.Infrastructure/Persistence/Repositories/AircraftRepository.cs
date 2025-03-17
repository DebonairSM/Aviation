using EnterpriseApiIntegration.Domain.Aircraft;
using EnterpriseApiIntegration.Application.Features.Aircraft.FakeData;

namespace EnterpriseApiIntegration.Infrastructure.Persistence.Repositories;

public class AircraftRepository : IAircraftRepository
{
    private readonly ApplicationDbContext _context;
    private static readonly List<Aircraft> _demoData = AircraftFaker.CreateAircraftFaker()
        .Generate(10)
        .Select(dto => new Aircraft
        {
            Id = dto.Id,
            Registration = dto.Registration,
            Type = dto.Type,
            Manufacturer = dto.Manufacturer,
            Model = dto.Model,
            YearOfManufacture = dto.YearOfManufacture,
            SerialNumber = dto.SerialNumber,
            Status = dto.Status,
            LastMaintenanceDate = dto.LastMaintenanceDate,
            NextMaintenanceDue = dto.NextMaintenanceDue,
            TotalFlightHours = dto.TotalFlightHours
        })
        .ToList();

    public AircraftRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Aircraft> GetByIdAsync(int id)
    {
        return await Task.FromResult(_demoData.FirstOrDefault(a => a.Id == id));
    }

    public async Task<IEnumerable<Aircraft>> GetAllAsync()
    {
        return await Task.FromResult(_demoData);
    }

    public async Task<Aircraft> AddAsync(Aircraft aircraft)
    {
        aircraft.Id = _demoData.Max(a => a.Id) + 1;
        _demoData.Add(aircraft);
        return await Task.FromResult(aircraft);
    }

    public async Task<Aircraft> UpdateAsync(Aircraft aircraft)
    {
        var existingAircraft = _demoData.FirstOrDefault(a => a.Id == aircraft.Id);
        if (existingAircraft != null)
        {
            var index = _demoData.IndexOf(existingAircraft);
            _demoData[index] = aircraft;
        }
        return await Task.FromResult(aircraft);
    }

    public async Task DeleteAsync(int id)
    {
        var aircraft = _demoData.FirstOrDefault(a => a.Id == id);
        if (aircraft != null)
        {
            _demoData.Remove(aircraft);
        }
        await Task.CompletedTask;
    }
} 