using KinoPoshuk.BLL.DTO;
using KinoPoshuk.BLL.Interfaces;
using KinoPoshuk.BLL.Mapping;
using KinoPoshuk.DAL.DTO;
using KinoPoshuk.DAL.Entities;
using KinoPoshuk.DAL.Exceptions;
using KinoPoshuk.DAL.Interfaces;

namespace KinoPoshuk.BLL.Services;

public class MovieService : IMovieService
{
    private readonly IOmdbApiClient _omdbApiClient;
    private readonly IFavoriteMovieRepository _favoriteMovieRepository;
    private readonly ISearchHistoryRepository _searchHistoryRepository;

    public MovieService(
        IOmdbApiClient omdbApiClient,
        IFavoriteMovieRepository favoriteMovieRepository,
        ISearchHistoryRepository searchHistoryRepository)
    {
        _omdbApiClient = omdbApiClient;
        _favoriteMovieRepository = favoriteMovieRepository;
        _searchHistoryRepository = searchHistoryRepository;
    }

    public async Task<MovieDto?> SearchAsync(string title)
    {
        var movie = await _omdbApiClient.GetByTitleAsync(title);

        await _searchHistoryRepository.AddAsync(new SearchHistoryEntry
        {
            Query = title,
            WasFound = movie is not null
        });
        await _searchHistoryRepository.SaveChangesAsync();

        return movie;
    }

    public async Task<IReadOnlyList<FavoriteMovieDto>> GetFavoritesAsync()
    {
        var favorites = await _favoriteMovieRepository.GetAllAsync();
        return favorites.Select(f => f.ToDto()).ToList();
    }

    public async Task<FavoriteMovieDto> AddFavoriteAsync(AddFavoriteRequestDto request)
    {
        var existing = await _favoriteMovieRepository.GetByImdbIdAsync(request.ImdbId);
        if (existing is not null) return existing.ToDto();

        var entity = request.ToEntity();
        await _favoriteMovieRepository.AddAsync(entity);
        await _favoriteMovieRepository.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task RemoveFavoriteAsync(int id)
    {
        var entity = await _favoriteMovieRepository.GetByIdAsync(id);
        if (entity is null) throw new MovieNotFoundException($"id={id}");

        _favoriteMovieRepository.Remove(entity);
        await _favoriteMovieRepository.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<SearchHistoryDto>> GetHistoryAsync()
    {
        var history = await _searchHistoryRepository.GetRecentAsync(20);
        return history.Select(h => h.ToDto()).ToList();
    }
}
