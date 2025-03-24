using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enterprise.Domain.Aircraft;

public interface IAircraftRepository
{
    Task<Aircraft?> GetByIdAsync(Guid id);
    Task<IEnumerable<Aircraft>> GetAllAsync();
    Task<Aircraft> AddAsync(Aircraft aircraft);
    Task<Aircraft> UpdateAsync(Aircraft aircraft);
    Task DeleteAsync(Guid id);
} 