using Integrator.DataAccess.Models.Complaints;
using Integrator.Frontend.WebInterface.Pages.Complaints;
using Integrator.Shared.ErrorHandling;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints;

public interface IComplaintDetailsViewModel
{
    string ComplaintNumber { get; set; }
    IComplaint Complaint { get; set; }
    void SetFileds(string number);
}

public class ComplaintDetailsViewModel : IComplaintDetailsViewModel
{
    public string ComplaintNumber { get; set; }
    public IComplaint Complaint { get; set; }
    public IComplaintsList ComplaintsList { get; set; }

    public ComplaintDetailsViewModel(IComplaintsList complaintsList)
    {
        ComplaintsList = complaintsList;
    }

    public void SetFileds(string number)
    {
        ComplaintNumber = number.Replace("-", "/");

        Complaint = ComplaintsList.AllComplaintsList
            .FirstOrDefault(c => c.Number == ComplaintNumber);
    }
}
