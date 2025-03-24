using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enterprise.Application.Features.Aircraft;

namespace Enterprise.Application.Interfaces;

public interface IAircraftService
{
    Task<AircraftDto> GetByIdAsync(Guid id);
    Task<IEnumerable<AircraftDto>> GetAllAsync();
    Task<AircraftDto> CreateAsync(AircraftDto aircraft);
    Task<AircraftDto> UpdateAsync(AircraftDto aircraft);
    Task DeleteAsync(Guid id);
} 