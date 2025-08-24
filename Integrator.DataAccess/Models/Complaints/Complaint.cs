using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "SWDR")]
        SWDR = 0,

        [Display(Name = "Symulator")]
        Symulator = 1,

        [Display(Name = "Forum")]
        Forum = 2,

        [Display(Name = "Chat")]
        Chat = 3,

        [Display(Name = "Other")]
        Other = 4
    }

    public string Number { get; set; } = null!; // Easy to write complaint number
    public string Description { get; set; } = null!;
    public ComplaintStatus Status { get; set; }
    public ComplaintContext Context { get; set; }
    
    public ICollection<InvolvedUser> InvolvedUsers { get; private set; } = new List<InvolvedUser>();
}