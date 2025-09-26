using Integrator.DataAccess.Models.Complaints;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Text.RegularExpressions;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints;

public interface IComplaintDetailsViewModel
{
    string ComplaintNumber { get; set; }
    IComplaint Complaint { get; set; }
    void SetFileds(string number);
    void AddNewResponse(string context);
}

public class ComplaintDetailsViewModel : IComplaintDetailsViewModel
{
    public string ComplaintNumber { get; set; }
    public IComplaint Complaint { get; set; }
    public IComplaintsList ComplaintsList { get; set; }
    private AuthenticationStateProvider AuthProvider { get; }

    public ComplaintDetailsViewModel(IComplaintsList complaintsList, AuthenticationStateProvider authProvider)
    {
        ComplaintsList = complaintsList;
        AuthProvider = authProvider;
    }

    public void SetFileds(string number)
    {
        ComplaintNumber = number.Replace("-", "/");

        Complaint = ComplaintsList.AllComplaintsList
            .FirstOrDefault(c => c.Number == ComplaintNumber);
    }

    public async void AddNewResponse(string context)
    {
        if (Complaint == null)
        {
            return;
        }
        var decoded = WebUtility.HtmlDecode(context);
        var text = Regex.Replace(decoded, "<.*?>", string.Empty);
        text = decoded;

        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var username = user.Identity?.Name ?? "XX";

        await Complaint.AddResponce(username, text);
    }
}
