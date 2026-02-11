namespace Integrator.DataAccess.Models.Complaints;

public interface IComplaintResponse
{
    public string UserId { get; set; }
    string ResponderName { get; set; }
    string Content { get; set; }
    DateTime ResponseDate { get; set; }
    string ContentUpdated { get; set; }
    DateTime ResponseDateUpdated { get; set; }
    Task UpdateResponse(string newContent);
}

public class ComplaintResponse : IComplaintResponse
{
    public string UserId { get; set; }
    public string ResponderName { get; set; }
    public string Content { get; set; }
    public DateTime ResponseDate { get; set; }
    public string ContentUpdated { get; set; }
    public DateTime ResponseDateUpdated { get; set; }
    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    public async Task UpdateResponse(string newContent)
    {
        await _lock.WaitAsync();
        try
        {
            ContentUpdated = newContent;
            ResponseDateUpdated = DateTime.UtcNow;
        }
        finally
        {
            _lock.Release();
        }
    }
}
