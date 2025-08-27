namespace Integrator.DataAccess.Models.Complaints;

public interface IComplaintsList
{
    List<IComplaint> AllComplaintsList { get; }
    void AddNewComplaint(IComplaint complaint);
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

    public void AddNewComplaint(IComplaint complaint)
    {
        lock (_lock)
        {
            var now = DateTime.Now;
            complaint.CreatedAt = now;
            complaint.UpdatedAt = now;
            complaint.Status = Complaint.ComplaintStatus.Open;
            SetIdForNewComplaint(complaint);

            AllComplaintsList.Add(complaint);
            ComplaintAdded?.Invoke(complaint);
        }
    }

    private async Task LoadComplaintsAsync()
    {
        AddNewComplaint(new Complaint()
        {
            Number = "1/2024",
            InvolvedUsers =
            {
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Accuser,
                    UserId = "1",
                    Nickname = "gagar"
                },
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Accused,
                    UserId = "2",
                    Nickname = "stonka"
                },
                new InvolvedUser()
                {
                    Role = InvolvedUser.InvolvedUserRole.Witness,
                    UserId = "3",
                    Nickname = "byk"
                }
            },
            Status = Complaint.ComplaintStatus.Open,
            Context = Complaint.ComplaintContext.Symulator,
            Description = "Używanie botów w symulatorze",
            CreatedAt = DateTime.Now.AddDays(-5),
            UpdatedAt = DateTime.Now.AddDays(-1)

        });
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

