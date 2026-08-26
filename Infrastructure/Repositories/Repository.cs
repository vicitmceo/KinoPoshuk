using KinoPoshuk.Domain.Interfaces;
using KinoPoshuk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KinoPoshuk.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly KinoPoshukDbContext Db;
    protected readonly DbSet<T> DbSet;

    public Repository(KinoPoshukDbContext db)
    {
        Db = db;
        DbSet = db.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id);

    public async Task<IReadOnlyList<T>> GetAllAsync() => await DbSet.AsNoTracking().ToListAsync();

    public async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

    public void Remove(T entity) => DbSet.Remove(entity);

    public async Task SaveChangesAsync() => await Db.SaveChangesAsync();
}
