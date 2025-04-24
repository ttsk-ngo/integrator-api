namespace Integrator.DataAccess.Models.Complaints;

public class Complaint : BaseModel
{
    public string Number { get; set; } = null!; // Easy to write complaint number
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Context { get; set; } = null!;
    
    public ICollection<InvolvedUser> InvolvedUsers { get; private set; } = new List<InvolvedUser>();
}