namespace KinoPoshuk.Application.DTO;

public class MovieDto
{
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Actors { get; set; } = string.Empty;
    public string Plot { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
    public string ImdbRating { get; set; } = string.Empty;
    public string Runtime { get; set; } = string.Empty;
}
