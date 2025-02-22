namespace Integrator.DataAccess.Models;

public class BaseModel
{
    public string Id { get; set; }
    public ulong CreatedAt { get; set; }
    public ulong UpdatedAt { get; set; }
}