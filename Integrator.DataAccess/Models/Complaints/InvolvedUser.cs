namespace Integrator.DataAccess.Models.Complaints;

public class InvolvedUser : BaseModel
{
    public enum InvolvedUserRole
    {
        NotSet = 0,
        AssignedModerator = 1,
        Accuser = 2,
        Accused = 3,
        Witness = 4
    }
    
    public string UserId { get; set; } = null!;
    public InvolvedUserRole Role { get; set; }
    public string? ViolatedRules { get; set; }
    
    public string? ComplaintId { get; set; }
    public Complaint? Complaint { get; private set; }
}