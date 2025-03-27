using Microsoft.EntityFrameworkCore;
using VaccinationService.DbContexts;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly VaccinationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(VaccinationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        IQueryable<T> query = _dbSet;

        if (include != null)
        {
            query = include(query);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    
    public async Task<T?> GetByIdAsync(params object[] keys)
    {
      var entity = await _dbSet.FindAsync(keys);
      if (entity != null)
      {
        _context.Entry(entity).State = EntityState.Detached; // Detach the entity
      }
      return entity;
    }


    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
