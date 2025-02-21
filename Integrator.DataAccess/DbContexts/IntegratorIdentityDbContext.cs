using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Integrator.DataAccess.DbContexts;

public class IntegratorIdentityDbContext : IdentityDbContext
{
    public IntegratorIdentityDbContext(DbContextOptions<IntegratorIdentityDbContext> options) : base(options)
    { }
}