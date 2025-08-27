using System.ComponentModel.DataAnnotations;
using static Integrator.DataAccess.Models.Complaints.Complaint;

namespace Integrator.DataAccess.Models.Complaints;

public class InvolvedUser : BaseModel
{
    public enum InvolvedUserRole
    {
        [Display(Name = "Not Set")]
        NotSet = 0,

        [Display(Name = "Assigned Moderator")]
        AssignedModerator = 1,

        [Display(Name = "Accuser")]
        Accuser = 2,

        [Display(Name = "Accused")]
        Accused = 3,

        [Display(Name = "Witness")]
        Witness = 4
    }
    
    public string UserId { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public InvolvedUserRole Role { get; set; }
    public string? ViolatedRules { get; set; }
    public ComplaintContext Context { get; set; }

    public string? ComplaintId { get; set; }
    public Complaint? Complaint { get; private set; }
}