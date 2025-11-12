using Integrator.DataAccess.Models.Complaints;
using Microsoft.EntityFrameworkCore;

namespace Integrator.DataAccess.DbContexts;

public class IntegratorDbContext : DbContext
{
    private const string BanComplaints = "ban_complaints";
    private const string BanInvolvedUsers = "ban_involvedUsers";

    public IntegratorDbContext(DbContextOptions<IntegratorDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Complaint>()
            .ToTable(BanComplaints)
            .HasMany<InvolvedUser>(x => x.InvolvedUsers)
            .WithOne(x => x.Complaint)
            .HasForeignKey(x => x.ComplaintId);

        modelBuilder.Entity<InvolvedUser>()
            .ToTable(BanInvolvedUsers);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Complaint> Complaints { get; set; }

    public DbSet<InvolvedUser> InvolvedUsers { get; set; }
}