using Ganss.Xss;
using Integrator.DataAccess.Models.Complaints;
using Integrator.Frontend.WebInterface.ViewModels.Editor;
using MudBlazor;
using System.Net;
using System.Text.RegularExpressions;
using static Integrator.DataAccess.Models.Complaints.Complaint;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints
{
    public interface IDialogNewComplaintViewModel
    {
        string? SelectedNickname { get; set; }
        string? SelectedRule { get; set; }
        string AccuserNickname { get; set; }
        string[] Nicknames { get; }
        ISnackbar Snackbar { get; }
        IComplaint ComplainData { get; }
        ComplaintContext? SelectedContext { get; set; }
        InvolvedUser.InvolvedUserRole? SelectedUserRole { get; set; }

        bool Submit(IRichTextEditorViewModel EditorViewModel);
        void AddUserToComplain();
        void RemoveUserFromComplain(InvolvedUser user);
        void Reset();
        string GetSelectedClass(InvolvedUser.InvolvedUserRole option);
        bool CheckDialogErrorBeforeSubmit(string? description);
        IComplaint GetComplainData();
        Variant GetButtonVariant(InvolvedUser.InvolvedUserRole option);
        Color GetButtonColor(InvolvedUser.InvolvedUserRole option);
        List<string> GetRulesForContext(ComplaintContext? context);
        Task<IEnumerable<string>> SearchNicknames(string value, CancellationToken token);
    }

    public class DialogNewComplaintViewModel : IDialogNewComplaintViewModel
    {

        public string? SelectedNickname { get; set; }
        public string? SelectedRule { get; set; }
        public string AccuserNickname { get; set; }
        public string[] Nicknames => nicknames;
        public ISnackbar Snackbar { get; }
        public IComplaint ComplainData { get; private set; }
        public ComplaintContext? SelectedContext { get; set; }
        public InvolvedUser.InvolvedUserRole? SelectedUserRole { get; set; }

        private readonly Dictionary<ComplaintContext?, List<string>> _rulesForContexts = new()
            {
                { ComplaintContext.Symulator, new List<string> {
                    "1. Zakaz używania botów w symulatorze",
                    "1.1. Zakaz używania makr do automatyzacji",
                    "1.2. Zakaz używania skryptów wspomagających",
                    "2. Zakaz oszukiwania w symulatorze",
                    "2.1. Zakaz wykorzystywania błędów gry",
                    "2.2. Zakaz manipulacji wynikami"
                }},
                { ComplaintContext.SWDR, new List<string> {
                    "1. Zakaz łamania kodeksu jazdy SWDR",
                    "1.1. Zakaz jazdy pod prąd",
                    "1.2. Zakaz przekraczania prędkości",
                    "2. Zakaz łamania zasad komunikacji",
                    "2.1. Zakaz używania nieprawidłowych komunikatów",
                    "2.2. Zakaz ignorowania dyspozytora"
                }},
                { ComplaintContext.Chat, new List<string> {
                    "1. Zakaz używania wulgaryzmów na czacie",
                    "1.1. Zakaz przekleństw",
                    "1.2. Zakaz obelg",
                    "2. Zakaz spamu na czacie",
                    "2.1. Zakaz wielokrotnego powtarzania wiadomości",
                    "2.2. Zakaz używania czatu do celów reklamowych"
                }},
                { ComplaintContext.Forum, new List<string> {
                    "1. Zakaz publikowania niewłaściwych treści",
                    "1.1. Zakaz treści wulgarnych",
                    "1.2. Zakaz treści politycznych",
                    "2. Zakaz spamu na forum",
                    "2.1. Zakaz tworzenia wielu tematów o tej samej treści",
                    "2.2. Zakaz publikowania reklam"
                }},
                { ComplaintContext.Other, new List<string> {
                    "1. Inne naruszenie regulaminu",
                    "1.1. Naruszenie kodeksu etycznego",
                    "1.2. Oszustwo"
                }}
            };

        private readonly string[] nicknames = [
                "trichlor", "Mr_bar", "Drozda32", "_l0stfake7", "xoorbes", "bagi2424"
                // ... other nicknames
            ];

        public DialogNewComplaintViewModel(ISnackbar snackbar)
        {
            Snackbar = snackbar;
            ComplainData = new Complaint();
            SelectedUserRole = InvolvedUser.InvolvedUserRole.Accused;
            AccuserNickname = string.Empty;
        }

        // Helper methods for button styling
        public string GetSelectedClass(InvolvedUser.InvolvedUserRole option)
        {
            return SelectedUserRole == option ? "mt-1 mb-1" : "m-1";
        }

        public Variant GetButtonVariant(InvolvedUser.InvolvedUserRole option)
        {
            return SelectedUserRole == option ? Variant.Filled : Variant.Outlined;
        }

        public Color GetButtonColor(InvolvedUser.InvolvedUserRole option)
        {
            if (SelectedUserRole != option)
                return Color.Default;

            return option switch
            {
                InvolvedUser.InvolvedUserRole.Accused => Color.Warning,
                InvolvedUser.InvolvedUserRole.Witness => Color.Info,
                _ => Color.Primary
            };
        }

        // Method returning rules for a given context
        public List<string> GetRulesForContext(ComplaintContext? context)
        {
            return _rulesForContexts.ContainsKey(context)
                ? _rulesForContexts[context]
                : new List<string>();
        }

        // Method adding a user to the complaint
        public void AddUserToComplain()
        {
            // Check if nickname is selected
            if (string.IsNullOrWhiteSpace(SelectedNickname))
                return;

            // If the user is accused, a violated rule must be selected
            if (SelectedUserRole == InvolvedUser.InvolvedUserRole.Accused &&
                string.IsNullOrWhiteSpace(SelectedRule))
            {
                // Use correct MudBlazor Snackbar syntax
                Snackbar.Add("Please select a violated rule for accused user", Severity.Error);
                return;
            }

            var alreadyAdded = ComplainData.InvolvedUsers
                .Any(u => u.Nickname.Equals(SelectedNickname)
                  && u.Role != SelectedUserRole);
            if (alreadyAdded)
            {
                Snackbar.Add("This user has already been added with a different role.", Severity.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedNickname) ||
                SelectedContext == null)
                return;

            // Determine violated rules
            string violatedRules = "";
            if (SelectedUserRole == InvolvedUser.InvolvedUserRole.Accused && !string.IsNullOrEmpty(SelectedRule))
            {
                violatedRules = SelectedRule;
            }

            var newUser = new InvolvedUser();
            newUser.Nickname = SelectedNickname;
            newUser.Role = SelectedUserRole ?? InvolvedUser.InvolvedUserRole.NotSet;
            newUser.Context = SelectedContext ?? ComplaintContext.Other;
            newUser.ViolatedRules = violatedRules;

            ComplainData.InvolvedUsers.Add(newUser);

            // Reset selection fields after adding
            SelectedNickname = null;
            SelectedRule = null;
            SelectedContext = null;
        }

        // Remove user from the complaint
        public void RemoveUserFromComplain(InvolvedUser user)
        {
            ComplainData.InvolvedUsers.Remove(user);
        }

        public IComplaint GetComplainData() => ComplainData;

        public void Reset()
        {
            SelectedNickname = null;
            SelectedContext = null;
            SelectedUserRole = InvolvedUser.InvolvedUserRole.Accused;
            SelectedRule = null;
            ComplainData = new Complaint();
        }

        public bool CheckDialogErrorBeforeSubmit(string? description)
        {
            var decoded = WebUtility.HtmlDecode(description);
            var text = Regex.Replace(decoded, "<.*?>", string.Empty);

            if (string.IsNullOrWhiteSpace(text))
            {
                Snackbar.Add("Description can not be empty.", Severity.Error);
                return false;
            }

            if (!ComplainData.IsAccusedSet())
            {
                Snackbar.Add("The accused user has not been set.", Severity.Error);
                return false;
            }

            return true;
        }

        public bool Submit(IRichTextEditorViewModel EditorViewModel)
        {
            var sanitizer = new HtmlSanitizer();
            var cleanHtml = sanitizer.Sanitize(EditorViewModel.EditorContent);

            if (!CheckDialogErrorBeforeSubmit(cleanHtml))
            {
                return false;
            }

            ComplainData.Description.Content = cleanHtml;
            ComplainData.Description.ContentUpdated = cleanHtml;
            ComplainData.Description.ResponseDate = DateTime.UtcNow;
            ComplainData.Description.ResponseDateUpdated = DateTime.UtcNow;

            EditorViewModel.EditorContent = String.Empty;

            return true;
        }

        // Search method for Autocomplete
        public async Task<IEnumerable<string>> SearchNicknames(string value, CancellationToken token)
        {
            await Task.Delay(5, token);

            if (string.IsNullOrEmpty(value))
                return Array.Empty<string>();

            // Filter nicknames, excluding those already in the table
            var alreadyAddedNicknames = ComplainData.InvolvedUsers
                .Where(u => u.Role != SelectedUserRole)
                .Select(u => u.Nickname)
                .ToHashSet();

            return Nicknames
                .Where(x => !alreadyAddedNicknames.Contains(x) && !x.Equals(AccuserNickname))
                .Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}

