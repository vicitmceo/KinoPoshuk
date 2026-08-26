using KinoPoshuk.Domain.Entities;

namespace KinoPoshuk.Domain.Interfaces;

public interface IFavoriteMovieRepository : IRepository<FavoriteMovie>
{
    Task<FavoriteMovie?> GetByImdbIdAsync(string imdbId);
}
