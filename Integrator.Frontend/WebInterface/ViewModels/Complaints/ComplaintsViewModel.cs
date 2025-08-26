using Integrator.DataAccess.Models.Complaints;
using Integrator.Frontend.WebInterface.Pages.Complaints;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints;

public interface IComplaintsViewModel
{
    List<IComplaint> Complaints { get; }
    Task LoadComplaintsAsync();
    void AddNewComplaint(IComplaint complaint);
    event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? propertyName = null);
}

public class ComplaintsViewModel : ViewModelBase, IComplaintsViewModel
{
    private List<IComplaint> _complaints = new();
    public List<IComplaint> Complaints
    {
        get => _complaints;
        private set => SetField(ref _complaints, value);
    }

    public async Task LoadComplaintsAsync()
    {
        _complaints = new List<IComplaint>()
        {
            new Complaint()
            {
                Number = "1/2025",
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
            }
        };
    }

    public void AddNewComplaint(IComplaint complaint)
    {
        var now = DateTime.Now;
        complaint.CreatedAt = now;
        complaint.UpdatedAt = now;
        complaint.Status = Complaint.ComplaintStatus.Open;
        
        _complaints.Add(complaint);
        OnPropertyChanged(nameof(Complaints));
    }
}