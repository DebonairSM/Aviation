using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enterprise.Domain.Aircraft;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Infrastructure.Persistence.Repositories;

public class AircraftRepository : IAircraftRepository
{
    private readonly ApplicationDbContext _context;

    public AircraftRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Aircraft.Aircraft?> GetByIdAsync(Guid id)
    {
        return await _context.Aircraft.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Domain.Aircraft.Aircraft>> GetAllAsync()
    {
        return await _context.Aircraft.ToListAsync();
    }

    public async Task<Domain.Aircraft.Aircraft> AddAsync(Domain.Aircraft.Aircraft aircraft)
    {
        await _context.Aircraft.AddAsync(aircraft);
        await _context.SaveChangesAsync();
        return aircraft;
    }

    public async Task<Domain.Aircraft.Aircraft> UpdateAsync(Domain.Aircraft.Aircraft aircraft)
    {
        _context.Aircraft.Update(aircraft);
        await _context.SaveChangesAsync();
        return aircraft;
    }

    public async Task DeleteAsync(Guid id)
    {
        var aircraft = await GetByIdAsync(id);
        if (aircraft is not null)
        {
            _context.Aircraft.Remove(aircraft);
            await _context.SaveChangesAsync();
        }
    }
} 