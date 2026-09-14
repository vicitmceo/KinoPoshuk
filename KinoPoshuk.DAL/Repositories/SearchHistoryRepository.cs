using KinoPoshuk.DAL.Data;
using KinoPoshuk.DAL.Entities;
using KinoPoshuk.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KinoPoshuk.DAL.Repositories;

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
