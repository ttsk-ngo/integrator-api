using Integrator.DataAccess.Models.Complaints;
using Integrator.DataAccess.Models.Users;
using Integrator.Shared.Helpers.Enums;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net;
using System.Text.RegularExpressions;
using static Integrator.DataAccess.Models.Complaints.Complaint;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints;

public interface IComplaintDetailsViewModel
{
    string ComplaintNumber { get; set; }
    string? SelectedNickname { get; set; }
    IComplaint Complaint { get; set; }
    ISnackbar Snackbar { get; }

    void SetFields(string number);
    Task AddNewResponse(string context);
    Task CloseOrOpenComplaint();
    Task AssignModerator();
    Task<bool> IsUserInRole(Roles role);
    Task<bool> CheckPermissionForOpenDialog();
    Task<IEnumerable<string>> SearchNicknames(string value, CancellationToken token);
}

public class ComplaintDetailsViewModel : IComplaintDetailsViewModel
{
    public string ComplaintNumber { get; set; }
    public string? SelectedNickname { get; set; }
    public IComplaint Complaint { get; set; }
    public ISnackbar Snackbar { get; }
    public IComplaintsList ComplaintsList { get; set; }
    private AuthenticationStateProvider AuthProvider { get; }
    private readonly string[] nicknames = [
                "marbas83", "Pepsi2026", "Krzysiek9825"
            ];

    public ComplaintDetailsViewModel(IComplaintsList complaintsList, AuthenticationStateProvider authProvider, ISnackbar snackbar)
    {
        ComplaintsList = complaintsList;
        AuthProvider = authProvider;
        Snackbar = snackbar;
    }

    public void SetFields(string number)
    {
        ComplaintNumber = number.Replace("-", "/");

        Complaint = ComplaintsList.AllComplaintsList
            .FirstOrDefault(c => c.Number == ComplaintNumber);
    }

    public async Task AddNewResponse(string context)
    {
        if (Complaint == null)
        {
            return;
        }
        if (Complaint.Status != ComplaintStatus.Open)
        {
            return;
        }

        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            return;
        }

        var username = user.Identity?.Name;

        if (string.IsNullOrEmpty(username))
        {
            return;
        }

        var responsesList = Complaint.Responses.ToList();
        var lastResponse = responsesList
            .Where(r => r.ResponderName == user.Identity.Name)
            .LastOrDefault();

        if (lastResponse != null)
        {
            if (lastResponse.ResponseDate >= DateTime.UtcNow.AddMinutes(-15))
            {
                Snackbar.Add("You cannot send responses more often than every 15 minutes.", Severity.Error);
                return;
            }
        }

        var decoded = WebUtility.HtmlDecode(context);
        var text = Regex.Replace(decoded, "<.*?>", string.Empty);

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        await Complaint.AddResponce(username, context);
    }

    public async Task CloseOrOpenComplaint()
    {
        if (IsUserInRole(Roles.Moderator).Result || IsUserInRole(Roles.HeadOfModerators).Result)
        {
            if (Complaint == null)
            {
                return;
            }
            if (Complaint.Status == ComplaintStatus.Open)
            {
                await Complaint.ChangeComplaintStatus(ComplaintStatus.Closed);
            }
            else if (Complaint.Status == ComplaintStatus.Closed)
            {
                await Complaint.ChangeComplaintStatus(ComplaintStatus.Open);
            }
        }
    }

    public async Task AssignModerator()
    {
        if (!IsUserInRole(Roles.HeadOfModerators).Result)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(SelectedNickname))
        {
            return;
        }

        await Complaint.AssignModerator(SelectedNickname);
    }

    public async Task<bool> IsUserInRole(Roles role)
    {
        var authState = await AuthProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
        var user = authState.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            return false;
        }

        return user.IsInRole(role.GetDisplayName());
    }

    public async Task<bool> CheckPermissionForOpenDialog()
    {
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState?.User;

        if (user == null || user.Identity?.IsAuthenticated != true)
        {
            return false;
        }
        if (!(IsUserInRole(Roles.Moderator).Result || IsUserInRole(Roles.HeadOfModerators).Result))
        {
            return false;
        }
        return true;
    }

    public async Task<IEnumerable<string>> SearchNicknames(string value, CancellationToken token)
    {
        await Task.Delay(5, token);

        if (string.IsNullOrEmpty(value))
            return Array.Empty<string>();

        var alreadyAddedNicknames = Complaint.InvolvedUsers
            .Select(u => u.Nickname)
            .ToHashSet();

        return nicknames
            .Where(x => !alreadyAddedNicknames.Contains(x))
            .Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
