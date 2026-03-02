using Integrator.DataAccess.DbContexts;
using Integrator.DataAccess.Models.Complaints;
using Microsoft.EntityFrameworkCore.Internal;

namespace Integrator.DataAccess.Repositories;

public class IntegratorDatabase : IIntegratorDatabase
{
    private readonly IntegratorDbContext _context;

    public IntegratorDatabase(IntegratorDbContext context)
    {
        _context = context;
        Complaints = new Repository<Complaint>(_context);
        InvolvedUsers = new Repository<InvolvedUser>(_context);
    }
    
    public IRepository<Complaint> Complaints { get; set; }
    public IRepository<InvolvedUser> InvolvedUsers { get; set; }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}