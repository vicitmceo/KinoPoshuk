using KinoPoshuk.DAL.Data;
using KinoPoshuk.DAL.Entities;
using KinoPoshuk.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KinoPoshuk.DAL.Repositories;

public class FavoriteMovieRepository : Repository<FavoriteMovie>, IFavoriteMovieRepository
{
    public FavoriteMovieRepository(KinoPoshukDbContext db) : base(db)
    {
    }

    public async Task<FavoriteMovie?> GetByImdbIdAsync(string imdbId) =>
        await DbSet.FirstOrDefaultAsync(f => f.ImdbId == imdbId);
}
