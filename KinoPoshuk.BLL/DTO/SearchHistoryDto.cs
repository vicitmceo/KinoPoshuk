namespace KinoPoshuk.BLL.DTO;

public class SearchHistoryDto
{
    public string Query { get; set; } = string.Empty;
    public bool WasFound { get; set; }
    public DateTime SearchedAt { get; set; }
}
