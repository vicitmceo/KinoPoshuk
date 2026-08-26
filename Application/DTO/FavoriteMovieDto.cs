namespace KinoPoshuk.Application.DTO;

public class FavoriteMovieDto
{
    public int Id { get; set; }
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string PosterUrl { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; }
}

public class AddFavoriteRequestDto
{
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string PosterUrl { get; set; } = string.Empty;
}
