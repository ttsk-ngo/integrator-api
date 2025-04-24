using Integrator.DataAccess.Models.Complaints;
using Microsoft.EntityFrameworkCore;

namespace Integrator.DataAccess.DbContexts;

public class IntegratorDbContext : DbContext
{
    public IntegratorDbContext(DbContextOptions<IntegratorDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Complaint>()
            .ToTable("ban_complaints")
            .HasMany<InvolvedUser>(x => x.InvolvedUsers)
            .WithOne(x => x.Complaint)
            .HasForeignKey(x => x.ComplaintId);

        modelBuilder.Entity<InvolvedUser>()
            .ToTable("ban_involvedUsers");
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Complaint> Complaints { get; set; }
    
    public DbSet<InvolvedUser> InvolvedUsers { get; set; }
}