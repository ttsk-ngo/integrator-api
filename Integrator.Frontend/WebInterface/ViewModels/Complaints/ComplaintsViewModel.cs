using Integrator.DataAccess.Models.Complaints;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints;

public interface IComplaintsViewModel
{
    IComplaintsList Complaints { get; }
    void AddNewComplaint(IComplaint complaint);
    //event PropertyChangedEventHandler? PropertyChanged;
}

public class ComplaintsViewModel : ViewModelBase, IComplaintsViewModel
{
    public ComplaintsViewModel(IComplaintsList complaintsList)
    {
        Complaints = complaintsList;
    }

    public IComplaintsList Complaints { get; private set; }

    public void AddNewComplaint(IComplaint complaint)
    {
        Complaints.AddNewComplaint(complaint);
    }
}