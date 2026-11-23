
using BloodPressureApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodPressureApp.API.Repositories;

public class BloodPressureRepository : GenericRepository<BloodPressure>
{
    public BloodPressureRepository(BloodpressureContext context)
        : base(context)
    {
    }

    public override async Task<List<BloodPressure>> GetAllAsync()
    {
        return await _dbSet
            .OrderByDescending(m => m.RecordedAt)
            .ThenByDescending(m => m.Id)
            .ToListAsync();
    }

    public async Task<List<BloodPressure>> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .Where(x => x.UserId == userId)
            .OrderByDescending(m => m.RecordedAt)
            .ThenByDescending(m => m.Id)
            .ToListAsync();
    }
}
