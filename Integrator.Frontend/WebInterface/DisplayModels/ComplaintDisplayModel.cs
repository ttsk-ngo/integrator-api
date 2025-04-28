using Integrator.DataAccess.Models.Complaints;

namespace Integrator.Frontend.WebInterface.DisplayModels;

public class ComplaintDisplayModel
{
    public string Number { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Complaint.ComplaintStatus Status { get; set; }
    public Complaint.ComplaintContext Context { get; set; }
    public ICollection<InvolvedUserDisplayModel> InvolvedUsers { get; private set; } = new List<InvolvedUserDisplayModel>();
}