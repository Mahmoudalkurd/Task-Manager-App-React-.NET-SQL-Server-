using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _ctx;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext ctx)
    {
        _ctx = ctx;
        _dbSet = ctx.Set<T>();
    }

    public async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(int id) =>
        await _dbSet.FindAsync(id);

    public void Update(T entity) =>
        _dbSet.Update(entity);

    public void Remove(T entity) =>
        _dbSet.Remove(entity);

    public async Task SaveChangesAsync() =>
        await _ctx.SaveChangesAsync();
}
