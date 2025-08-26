using System.ComponentModel.DataAnnotations;

namespace Integrator.DataAccess.Models.Complaints;

public interface IComplaint
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    string Number { get; set; }
    string Description { get; set; }
    Complaint.ComplaintStatus Status { get; set; }
    Complaint.ComplaintContext Context { get; set; }
    ICollection<InvolvedUser> InvolvedUsers { get; }
    string Accuser();
    string Accused();
    string Witnesses();
}


public class Complaint : BaseModel, IComplaint
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

    public string Number { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ComplaintStatus Status { get; set; }
    public ComplaintContext Context { get; set; }
    public ICollection<InvolvedUser> InvolvedUsers { get; private set; } = new List<InvolvedUser>();


    public string Accuser()
    {
        return InvolvedUsers
            .Where(u => u.Role == InvolvedUser.InvolvedUserRole.Accuser)
            .Select(u => u.Nickname)
            .First();
    }

    public string Accused()
    {
        return InvolvedUsers
            .Where(u => u.Role == InvolvedUser.InvolvedUserRole.Accused)
            .Select(u => u.Nickname)
            .First();
    }

    public string Witnesses()
    {
        var nicknames = InvolvedUsers
            .Where(u => u.Role == InvolvedUser.InvolvedUserRole.Accused)
            .Select(u => u.Nickname);

        return nicknames.First();
    }
}