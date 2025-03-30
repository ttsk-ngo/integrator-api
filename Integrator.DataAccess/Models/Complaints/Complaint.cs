namespace Integrator.DataAccess.Models.Complaints;

public class Complaint : BaseModel
{
    public string ComplaintNumber { get; set; } // Easy to write complaint number
    public string ComplaintDescription { get; set; }
}