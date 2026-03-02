namespace Integrator.DataAccess.Repositories;

public interface IRepository<TModel> : IDisposable where TModel : class
{
    Task<ICollection<TModel>> GetAll();
    Task<TModel?> GetById(string id);
    Task Add(TModel model);
    void Remove(string id);
}