using KinoPoshuk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KinoPoshuk.Infrastructure.Data;

public class KinoPoshukDbContext : DbContext
{
    public KinoPoshukDbContext(DbContextOptions<KinoPoshukDbContext> options) : base(options)
    {
    }

    public DbSet<FavoriteMovie> FavoriteMovies => Set<FavoriteMovie>();
    public DbSet<SearchHistoryEntry> SearchHistoryEntries => Set<SearchHistoryEntry>();
}
