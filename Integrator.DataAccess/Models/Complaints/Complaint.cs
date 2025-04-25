namespace Integrator.DataAccess.Models.Complaints;

public class Complaint : BaseModel
{
    public enum ComplaintStatus
    {
        NotSet = 0,
        Open = 1,
        Closed = 2
    }

    public enum ComplaintContext
    {
        NotSet = 0,
        SWDR = 1,
        TrainDriver2 = 2,
        Forum = 3
    }
    
    public string Number { get; set; } = null!; // Easy to write complaint number
    public string Description { get; set; } = null!;
    public ComplaintStatus Status { get; set; }
    public ComplaintContext Context { get; set; }
    
    public ICollection<InvolvedUser> InvolvedUsers { get; private set; } = new List<InvolvedUser>();
}