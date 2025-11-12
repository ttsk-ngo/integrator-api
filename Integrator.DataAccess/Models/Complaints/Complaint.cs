using Integrator.Shared.Helpers.Enums;
using Nextended.Core.Extensions;
using System.ComponentModel.DataAnnotations;
using static Integrator.DataAccess.Models.Complaints.Complaint;

namespace Integrator.DataAccess.Models.Complaints;

public interface IComplaint
{
    string Id { get; set; }
    DateTime CreatedAt { get; set; } //UTC
    DateTime UpdatedAt { get; } //UTC
    string Number { get; set; }
    IComplaintResponse Description { get; set; }
    ComplaintStatus Status { get; set; }
    ICollection<ComplaintContext> Context { get; set; }
    ICollection<InvolvedUser> InvolvedUsers { get; }
    string Accuser();
    string Accused();
    string Witnesses();
    bool IsAccusedSet();
    string ContextToString();
    ICollection<IComplaintResponse> Responses { get; set; }
    Task AddResponce(string username, string context);
    Task ChangeComplaintStatus(ComplaintStatus newStatus);
    Task AssignModerator(string username);
    Task ChangeUpdateTimeToNow();
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
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; } //UTC
    public DateTime UpdatedAt { get; private set; } //UTC
    public string Number { get; set; } = null!;
    public IComplaintResponse Description { get; set; } = new ComplaintResponse();
    public ComplaintStatus Status { get; set; }
    public ICollection<ComplaintContext> Context { get; set; } = new List<ComplaintContext>();
    public ICollection<InvolvedUser> InvolvedUsers { get; private set; } = new List<InvolvedUser>();
    public ICollection<IComplaintResponse> Responses { get; set; } = new List<IComplaintResponse>();
    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    public string Accuser()
    {
        return InvolvedUsers
            .Where(u => u.Role == InvolvedUser.InvolvedUserRole.Accuser)
            .Select(u => u.Nickname)
            .FirstOrDefault(@"b\d");
    }

    public string Accused()
    {
        var nicknames = InvolvedUsers
            .Where(u => u.Role == InvolvedUser.InvolvedUserRole.Accused)
            .Select(u => u.Nickname)
            .Distinct();

        return string.Join("; ", nicknames.Select(c => c));
    }

    public string Witnesses()
    {
        var nicknames = InvolvedUsers
            .Where(u => u.Role == InvolvedUser.InvolvedUserRole.Witness)
            .Select(u => u.Nickname)
            .Distinct();

        return string.Join("; ", nicknames.Select(c => c));
    }

    public bool IsAccusedSet()
    {
        return InvolvedUsers
            .Where(u => u.Role == InvolvedUser.InvolvedUserRole.Accused)
            .Any();
    }

    public string ContextToString()
    {
        return string.Join("; ", Context.Select(c => c.GetDisplayName()));
    }

    public async Task AddResponce(string username, string context)
    {
        await _lock.WaitAsync();
        try
        {
            Responses.Add(new ComplaintResponse()
            {
                ResponderName = username,
                Content = context,
                ResponseDate = DateTime.UtcNow,
                ContentUpdated = context,
                ResponseDateUpdated = DateTime.UtcNow
            });
            UpdatedAt = DateTime.UtcNow;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task ChangeComplaintStatus(ComplaintStatus newStatus)
    {
        await _lock.WaitAsync();
        try
        {
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task AssignModerator(string username)
    {
        if(Status != ComplaintStatus.Open)
        {
            return;
        }

        await _lock.WaitAsync();
        try
        {
            InvolvedUsers.RemoveAll(u => u.Role == InvolvedUser.InvolvedUserRole.AssignedModerator);
            InvolvedUsers.Add(new InvolvedUser()
            {
                Nickname = username,
                Role = InvolvedUser.InvolvedUserRole.AssignedModerator,
            });
            UpdatedAt = DateTime.UtcNow;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task ChangeUpdateTimeToNow()
    {
        await _lock.WaitAsync();
        try
        {
            UpdatedAt = DateTime.UtcNow;
        }
        finally
        {
            _lock.Release();
        }
    }
}