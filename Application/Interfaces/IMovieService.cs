using KinoPoshuk.Application.DTO;

namespace KinoPoshuk.Application.Interfaces;

public interface IMovieService
{
    Task<MovieDto?> SearchAsync(string title);
    Task<IReadOnlyList<FavoriteMovieDto>> GetFavoritesAsync();
    Task<FavoriteMovieDto> AddFavoriteAsync(AddFavoriteRequestDto request);
    Task RemoveFavoriteAsync(int id);
    Task<IReadOnlyList<SearchHistoryDto>> GetHistoryAsync();
}
