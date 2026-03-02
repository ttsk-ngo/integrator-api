using Integrator.DataAccess.Models.Complaints;
using static Integrator.DataAccess.Models.Complaints.Complaint;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints
{
    public interface IDialogCloseComplaintViewModel
    {
        public IComplaint Complaint { get; set; }
        public ComplaintStatus Decision { get; set; }
        public string AccusedMessage { get; set; }
        public string ModeratorNotes { get; set; }
        public int BanDurationDays { get; set; }
        public bool IsPermanent { get; set; }
        public Dictionary<ComplaintContext, bool> SubsystemSelections { get; set; }
        public string GetActionName(ComplaintStatus status);
        public int GetFinalBanDuration();
    }

    public class DialogCloseComplaintViewModel : IDialogCloseComplaintViewModel
    {
        public IComplaint Complaint { get; set; }
        public ComplaintStatus Decision { get; set; }
        public string AccusedMessage { get; set; }
        public string ModeratorNotes { get; set; }
        public int BanDurationDays { get; set; }
        public bool IsPermanent { get; set; }
        public Dictionary<ComplaintContext, bool> SubsystemSelections { get; set; } =
        Enum.GetValues(typeof(ComplaintContext))
            .Cast<ComplaintContext>()
            .ToDictionary(e => e, _ => false);
        public string GetActionName(ComplaintStatus status) => status switch
        {
            ComplaintStatus.Closed => "Close",
            ComplaintStatus.Rejected => "Reject",
            ComplaintStatus.Expired => "Expire",
            _ => status.ToString()
        };
        public int GetFinalBanDuration() => IsPermanent ? -1 : BanDurationDays;
    }
}
