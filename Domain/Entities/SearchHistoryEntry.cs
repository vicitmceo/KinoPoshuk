namespace KinoPoshuk.Domain.Entities;

public class SearchHistoryEntry
{
    public int Id { get; set; }
    public string Query { get; set; } = string.Empty;
    public bool WasFound { get; set; }
    public DateTime SearchedAt { get; set; } = DateTime.UtcNow;
}
