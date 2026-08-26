using KinoPoshuk.Application.DTO;

namespace KinoPoshuk.Application.Interfaces;

public interface IOmdbApiClient
{
    Task<MovieDto?> GetByTitleAsync(string title);
}
