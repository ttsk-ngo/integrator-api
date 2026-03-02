using Ganss.Xss;
using Integrator.DataAccess.Models.Complaints;
using Integrator.DataAccess.Models.Users;
using Integrator.Frontend.WebInterface.ViewModels.Editor;
using Integrator.Shared.Helpers.Enums;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static Integrator.DataAccess.Models.Complaints.Complaint;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints
{
    public interface IDialogEditResponseViewModel
    {
        public IComplaint Complaint { get; set; }
        public IComplaintResponse Response { get; set; }
        public ISnackbar Snackbar { get; }

        Task<bool> CheckDialogErrorBeforeSubmit(string? newMessage);
        Task<bool> Submit(IRichTextEditorViewModel EditorViewModel);
    }

    public class DialogEditResponseViewModel : IDialogEditResponseViewModel
    {
        public IComplaint Complaint { get; set; }
        public IComplaintResponse Response { get; set; }
        public ISnackbar Snackbar { get; }
        private AuthenticationStateProvider AuthProvider { get; }

        public DialogEditResponseViewModel(AuthenticationStateProvider authProvider, ISnackbar snackbar)
        {
            AuthProvider = authProvider;
            Snackbar = snackbar;
        }

        public async Task<bool> CheckDialogErrorBeforeSubmit(string? newMessage)
        {
            var authState = await AuthProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user == null || user.Identity?.IsAuthenticated == false)
            {
                Snackbar.Add("Not enough permissions.", Severity.Error);
                return false;
            }

            bool isModerator = user.IsInRole(Roles.Moderator.GetDisplayName()) ||
                       user.IsInRole(Roles.HeadOfModerators.GetDisplayName());

            if (!isModerator)
            {
                var currentUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                bool isAuthor = Response.UserId == currentUserId;
                bool isRecent = (DateTime.UtcNow - Response.ResponseDate).TotalMinutes <= 15;
                bool isLastMessage = Response.Equals(Complaint.Responses.LastOrDefault());

                if (!isAuthor || !isRecent || !isLastMessage)
                {
                    Snackbar.Add("You do not have permission to edit this message (time has expired or there is a new reply).", Severity.Error);
                    return false;
                }
            }

            var decoded = WebUtility.HtmlDecode(newMessage);
            var text = Regex.Replace(decoded, "<.*?>", string.Empty);

            if (string.IsNullOrWhiteSpace(text))
            {
                Snackbar.Add("New message can not be empty.", Severity.Error);
                return false;
            }

            return true;
        }

        public async Task<bool> Submit(IRichTextEditorViewModel EditorViewModel)
        {
            if (Complaint.Status != ComplaintStatus.Open)
            {
                return false;
            }

            var sanitizer = new HtmlSanitizer();
            var cleanHtml = sanitizer.Sanitize(EditorViewModel.EditorContent);
            bool checkResult = await CheckDialogErrorBeforeSubmit(cleanHtml);
            if (!checkResult)
            {
                return false;
            }

            await Response.UpdateResponse(cleanHtml);

            EditorViewModel.EditorContent = String.Empty;
            return true;
        }
    }
}

