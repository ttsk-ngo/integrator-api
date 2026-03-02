using Integrator.DataAccess.Models.Complaints;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints;

public interface IComplaintsViewModel
{
    IComplaintsList Complaints { get; }
}

public class ComplaintsViewModel : ViewModelBase, IComplaintsViewModel
{
    public ComplaintsViewModel(IComplaintsList complaintsList)
    {
        Complaints = complaintsList;
    }

    public IComplaintsList Complaints { get; private set; }
}