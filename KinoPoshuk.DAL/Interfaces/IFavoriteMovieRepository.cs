using KinoPoshuk.DAL.Entities;

namespace KinoPoshuk.DAL.Interfaces;

public interface IFavoriteMovieRepository : IRepository<FavoriteMovie>
{
    Task<FavoriteMovie?> GetByImdbIdAsync(string imdbId);
}
