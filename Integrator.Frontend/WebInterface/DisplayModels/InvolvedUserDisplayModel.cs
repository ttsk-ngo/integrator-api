using Integrator.DataAccess.Models.Complaints;

namespace Integrator.Frontend.WebInterface.DisplayModels;

public class InvolvedUserDisplayModel
{
    public string UserId { get; set; } = null!;
    public string Username { get; set; } = null!;
    public InvolvedUser.InvolvedUserRole Role { get; set; }
    public string? ViolatedRules { get; set; }
}