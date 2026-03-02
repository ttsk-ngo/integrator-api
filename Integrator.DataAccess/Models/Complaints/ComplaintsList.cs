using Integrator.Shared.Helpers.Enums;

namespace Integrator.DataAccess.Models.Complaints;

public interface IComplaintsList
{
    List<IComplaint> AllComplaintsList { get; }
    void AddNewComplaint(IComplaint complaint, string username, string userId);
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
        _ = LoadComplaintsAsync();
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
                .Where(c => c.HasValue)
                .Select(c => c.Value)
                .Distinct()
                .OrderBy(c => c.GetDisplayName())
                .ToList();

            var now = DateTime.UtcNow;
            complaint.CreatedAt = now;
            complaint.ChangeUpdateTimeToNow();
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
                    Context = Complaint.ComplaintContext.Symulator,
                    ViolatedRules = "Test 1"
                },
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Accused,
                    UserId = "2",
                    Nickname = "stonka",
                    Context = Complaint.ComplaintContext.SWDR,
                    ViolatedRules = "Test 2"
                },
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Accused,
                    UserId = "5",
                    Nickname = "turboStonka",
                    Context = Complaint.ComplaintContext.Other,
                    ViolatedRules = "Test 3"
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
            Responses = Enumerable.Range(0, 20)
                .Select(i => new ComplaintResponse()
                {
                    ResponderName = "marbas83",
                    Content = "Dziękujemy za zgłoszenie, przyjrzymy się sprawie.",
                    ResponseDate = DateTime.UtcNow.AddMinutes(i),
                    ContentUpdated = "Dziękujemy za zgłoszenie, przyjrzymy się sprawie.",
                    ResponseDateUpdated = DateTime.UtcNow.AddMinutes(i)
                })
                .Cast<IComplaintResponse>()
                .ToList(),

            Description = new ComplaintResponse()
            {
                ResponderName = "gagarZBipom",
                Content = "Używanie botów w symulatorze",
                ResponseDate = DateTime.UtcNow,
                ContentUpdated = "Używanie botów w symulatorze",
                ResponseDateUpdated = DateTime.UtcNow
            }

        }, "gagarZBipom", "761");
    }

    private void SetIdForNewComplaint(IComplaint complaint)
    {
        int year = DateTime.UtcNow.Year;

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

