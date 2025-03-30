using Integrator.DataAccess.Models.Complaints;
using Microsoft.EntityFrameworkCore;

namespace Integrator.DataAccess.DbContexts;

public class IntegratorDbContext : DbContext
{
    public IntegratorDbContext(DbContextOptions<IntegratorDbContext> options) : base(options)
    {}
    
    public DbSet<Complaint> Complaints { get; set; }
}