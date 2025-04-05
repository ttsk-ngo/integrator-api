namespace Integrator.DataAccess.Repositories;

public interface IRepository<TModel>
{
    public TModel GetAll();
    public TModel GetById(string id);
    public TModel Add(TModel model);
    public TModel Update(TModel model);
    public TModel Remove(string id);
}