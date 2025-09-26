using Integrator.DataAccess.Models.Complaints;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints;

public interface IComplaintsViewModel
{
    IComplaintsList Complaints { get; }
    void AddNewComplaint(IComplaint complaint, string username, string userId = "0");
}

public class ComplaintsViewModel : ViewModelBase, IComplaintsViewModel
{
    public ComplaintsViewModel(IComplaintsList complaintsList)
    {
        Complaints = complaintsList;
    }

    public IComplaintsList Complaints { get; private set; }

    public void AddNewComplaint(IComplaint complaint, string username, string userId = "0")
    {
        Complaints.AddNewComplaint(complaint, username, userId);
    }

    //ogarnąć te kolory przycisków
    //spytać gemini / chata o ten singleton czy da się inaczej zrobić tą listę skarg
}