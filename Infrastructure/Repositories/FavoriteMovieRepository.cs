using KinoPoshuk.Domain.Entities;
using KinoPoshuk.Domain.Interfaces;
using KinoPoshuk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KinoPoshuk.Infrastructure.Repositories;

public class FavoriteMovieRepository : Repository<FavoriteMovie>, IFavoriteMovieRepository
{
    public FavoriteMovieRepository(KinoPoshukDbContext db) : base(db)
    {
    }

    public async Task<FavoriteMovie?> GetByImdbIdAsync(string imdbId) =>
        await DbSet.FirstOrDefaultAsync(f => f.ImdbId == imdbId);
}
