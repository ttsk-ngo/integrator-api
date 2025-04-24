namespace Integrator.DataAccess.Models.Complaints;

public class InvolvedUser : BaseModel
{
    public string UserId { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string ViolatedRules { get; set; } = null!;
    
    public string? ComplaintId { get; set; }
    public Complaint? Complaint { get; private set; }
}