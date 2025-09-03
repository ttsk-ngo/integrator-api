using Integrator.Shared.Helpers.Enums;

namespace Integrator.DataAccess.Models.Complaints;

public interface IComplaintsList
{
    List<IComplaint> AllComplaintsList { get; }
    void AddNewComplaint(IComplaint complaint, string username, string userId = "0");
    event Action<IComplaint>? ComplaintAdded;
}

public class ComplaintsList : IComplaintsList
{
    public List<IComplaint> AllComplaintsList { get; private set; }
    private readonly object _lock;
    public event Action<IComplaint>? ComplaintAdded;

    public ComplaintsList()
    {
        AllComplaintsList = new List<IComplaint>();
        _lock = new object();
        LoadComplaintsAsync();
    }

    public void AddNewComplaint(IComplaint complaint, string username, string userId = "0")
    {
        lock (_lock)
        {
            var accuser = new InvolvedUser()
            {
                Role = InvolvedUser.InvolvedUserRole.Accuser,
                UserId = userId,
                Nickname = username
            };

            var contexts = complaint.InvolvedUsers
                .Where(u => u.Role == InvolvedUser.InvolvedUserRole.Accused || u.Role == InvolvedUser.InvolvedUserRole.Witness)
                .Select(u => u.Context)
                .Distinct()
                .OrderBy(c => c.GetDisplayName())
                .ToList();

            var now = DateTime.Now;
            complaint.CreatedAt = now;
            complaint.UpdatedAt = now;
            complaint.Status = Complaint.ComplaintStatus.Open;
            complaint.InvolvedUsers.Add(accuser);
            complaint.Context = contexts;
            SetIdForNewComplaint(complaint);

            AllComplaintsList.Add(complaint);
            ComplaintAdded?.Invoke(complaint);
        }
    }

    private async Task LoadComplaintsAsync()
    {
        AddNewComplaint(new Complaint()
        {
            InvolvedUsers =
            {
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Accused,
                    UserId = "2",
                    Nickname = "stonka",
                    Context = Complaint.ComplaintContext.Symulator
                },
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Accused,
                    UserId = "2",
                    Nickname = "stonka",
                    Context = Complaint.ComplaintContext.SWDR
                },
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Accused,
                    UserId = "5",
                    Nickname = "turboStonka",
                    Context = Complaint.ComplaintContext.Other
                },
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Witness,
                    UserId = "3",
                    Nickname = "byk",
                    Context = Complaint.ComplaintContext.Chat
                },
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Witness,
                    UserId = "4",
                    Nickname = "superSkladDoLwowka",
                    Context = Complaint.ComplaintContext.Forum
                },
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Witness,
                    UserId = "4",
                    Nickname = "superSkladDoLwowka",
                    Context = Complaint.ComplaintContext.Chat
                }
            },
            Description = "Używanie botów w symulatorze",

        }, "gagarZBipom", "761");
    }

    private void SetIdForNewComplaint(IComplaint complaint)
    {
        int year = DateTime.Now.Year;

        var currentYearComplaints = AllComplaintsList
            .Where(c => c.Number != null && c.Number.EndsWith($"/{year}"));

        int maxId = 0;
        foreach (var comp in currentYearComplaints)
        {
            var parts = comp.Number.Split('/');
            if (parts.Length == 2 && int.TryParse(parts[0], out int id))
            {
                if (id > maxId)
                    maxId = id;
            }
        }

        complaint.Number = $"{maxId + 1}/{year}";
    }
}

