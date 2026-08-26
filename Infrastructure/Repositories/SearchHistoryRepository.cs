using KinoPoshuk.Domain.Entities;
using KinoPoshuk.Domain.Interfaces;
using KinoPoshuk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KinoPoshuk.Infrastructure.Repositories;

public class SearchHistoryRepository : Repository<SearchHistoryEntry>, ISearchHistoryRepository
{
    public SearchHistoryRepository(KinoPoshukDbContext db) : base(db)
    {
    }

    public async Task<IReadOnlyList<SearchHistoryEntry>> GetRecentAsync(int count) =>
        await DbSet.AsNoTracking()
            .OrderByDescending(h => h.SearchedAt)
            .Take(count)
            .ToListAsync();
}
