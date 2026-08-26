using KinoPoshuk.Domain.Entities;

namespace KinoPoshuk.Domain.Interfaces;

public interface ISearchHistoryRepository : IRepository<SearchHistoryEntry>
{
    Task<IReadOnlyList<SearchHistoryEntry>> GetRecentAsync(int count);
}
