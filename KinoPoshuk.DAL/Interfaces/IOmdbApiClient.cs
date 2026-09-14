using KinoPoshuk.DAL.DTO;

namespace KinoPoshuk.DAL.Interfaces;

public interface IOmdbApiClient
{
    Task<MovieDto?> GetByTitleAsync(string title);
}
