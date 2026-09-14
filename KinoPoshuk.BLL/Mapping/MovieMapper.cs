using KinoPoshuk.BLL.DTO;
using KinoPoshuk.DAL.Entities;

namespace KinoPoshuk.BLL.Mapping;

public static class MovieMapper
{
    public static FavoriteMovieDto ToDto(this FavoriteMovie entity)
    {
        return new FavoriteMovieDto
        {
            Id = entity.Id,
            ImdbId = entity.ImdbId,
            Title = entity.Title,
            Year = entity.Year,
            PosterUrl = entity.PosterUrl,
            AddedAt = entity.AddedAt
        };
    }

    public static FavoriteMovie ToEntity(this AddFavoriteRequestDto dto)
    {
        return new FavoriteMovie
        {
            ImdbId = dto.ImdbId,
            Title = dto.Title,
            Year = dto.Year,
            PosterUrl = dto.PosterUrl
        };
    }

    public static SearchHistoryDto ToDto(this SearchHistoryEntry entity)
    {
        return new SearchHistoryDto
        {
            Query = entity.Query,
            WasFound = entity.WasFound,
            SearchedAt = entity.SearchedAt
        };
    }
}
