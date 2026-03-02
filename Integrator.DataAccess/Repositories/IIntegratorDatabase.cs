using Integrator.DataAccess.Models.Complaints;

namespace Integrator.DataAccess.Repositories;

public interface IIntegratorDatabase : IDisposable
{
    IRepository<Complaint> Complaints { get; set; }
    IRepository<InvolvedUser> InvolvedUsers { get; set; }
    Task SaveChangesAsync();
}