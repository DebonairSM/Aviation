using EnterpriseApiIntegration.Application.Features.Aircraft;

namespace EnterpriseApiIntegration.Application.Interfaces;

public interface IAircraftService
{
    Task<AircraftDto> GetByIdAsync(int id);
    Task<IEnumerable<AircraftDto>> GetAllAsync();
    Task<AircraftDto> CreateAsync(AircraftDto aircraft);
    Task<AircraftDto> UpdateAsync(AircraftDto aircraft);
    Task DeleteAsync(int id);
} 