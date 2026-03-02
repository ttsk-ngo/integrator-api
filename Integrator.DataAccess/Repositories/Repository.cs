using Integrator.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Integrator.DataAccess.Repositories;

public class Repository<TModel> : IRepository<TModel>, IAsyncDisposable where TModel : class
{
    private readonly IntegratorDbContext _context;

    public Repository(IntegratorDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<TModel>> GetAll()
    {
        return await _context.Set<TModel>().ToListAsync();
    }

    public async Task<TModel?> GetById(string id)
    {
        return await _context.Set<TModel>().FindAsync(id);
    }

    public async Task Add(TModel model)
    {
        await _context.Set<TModel>().AddAsync(model);
    }

    public void Remove(string id)
    {
        var entity = _context.Set<TModel>().Find(id);
        if (entity != null) 
            _context.Set<TModel>().Remove(entity);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}