using KinoPoshuk.DAL.Entities;

namespace KinoPoshuk.DAL.Interfaces;

public interface ISearchHistoryRepository : IRepository<SearchHistoryEntry>
{
    Task<IReadOnlyList<SearchHistoryEntry>> GetRecentAsync(int count);
}
