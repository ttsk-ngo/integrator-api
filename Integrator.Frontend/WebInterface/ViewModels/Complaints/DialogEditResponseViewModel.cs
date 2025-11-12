using Ganss.Xss;
using Integrator.DataAccess.Models.Complaints;
using Integrator.Frontend.WebInterface.ViewModels.Editor;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Extensions.Components;
using System.Net;
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
        private AuthenticationStateProvider AuthProvider { get; set; }

        public DialogEditResponseViewModel(AuthenticationStateProvider authProvider)
        {
            AuthProvider = authProvider;
        }

        public async Task<bool> CheckDialogErrorBeforeSubmit(string? newMessage)
        {
            var authState = await AuthProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            var requiredRoles = new[] { "Moderator", "Naczelnik moderatorow" };

            if (user == null || user.Identity?.IsAuthenticated == false || !requiredRoles.Any(role => user.IsInRole(role)))
            {
                Snackbar.Add("Not enough permissions.", Severity.Error);
                return false;
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

