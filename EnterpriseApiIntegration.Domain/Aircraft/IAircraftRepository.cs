namespace EnterpriseApiIntegration.Domain.Aircraft;

public interface IAircraftRepository
{
    Task<Aircraft> GetByIdAsync(int id);
    Task<IEnumerable<Aircraft>> GetAllAsync();
    Task<Aircraft> AddAsync(Aircraft aircraft);
    Task<Aircraft> UpdateAsync(Aircraft aircraft);
    Task DeleteAsync(int id);
} 