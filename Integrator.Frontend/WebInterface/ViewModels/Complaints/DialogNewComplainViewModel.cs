using Integrator.DataAccess.Models.Complaints;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Integrator.DataAccess.Models.Complaints.Complaint;

namespace Integrator.Frontend.WebInterface.ViewModels.Complaints
{
    public interface IDialogNewComplainViewModel
    {
        ISnackbar Snackbar { get; }
        string? SelectedNickname { get; set; }
        ComplaintContext? SelectedContext { get; set; }
        InvolvedUser.InvolvedUserRole? SelectedUserRole { get; set; }
        string? SelectedRule { get; set; }
        Complaint ComplainData { get; }
        string[] Nicknames { get; }
        Task<IEnumerable<string>> SearchNicknames(string value, CancellationToken token);
        string GetSelectedClass(InvolvedUser.InvolvedUserRole option);
        Variant GetButtonVariant(InvolvedUser.InvolvedUserRole option);
        Color GetButtonColor(InvolvedUser.InvolvedUserRole option);
        List<string> GetRulesForContext(ComplaintContext? context);
        void AddUserToComplain();
        void RemoveUserFromComplain(InvolvedUser user);
        Complaint GetComplainData();
    }

    public class DialogNewComplainViewModel : IDialogNewComplainViewModel
    {
        private readonly ISnackbar _snackbar;

        public ISnackbar Snackbar => _snackbar;

        public string? SelectedNickname { get; set; }
        public ComplaintContext? SelectedContext { get; set; }
        public InvolvedUser.InvolvedUserRole? SelectedUserRole { get; set; }
        public string? SelectedRule { get; set; }

        public DialogNewComplainViewModel(ISnackbar snackbar)
        {
            _snackbar = snackbar;

            SelectedUserRole = InvolvedUser.InvolvedUserRole.Accuser;
        }
        public Complaint ComplainData { get; private set; } = new Complaint();

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

        // Data sources
        private readonly string[] nicknames = {
                "trichlor", "Mr_bar", "Drozda32", "_l0stfake7", "xoorbes",
                // ... other nicknames
            };

        public string[] Nicknames => nicknames;

        // Search method for Autocomplete
        public async Task<IEnumerable<string>> SearchNicknames(string value, CancellationToken token)
        {
            await Task.Delay(5, token);

            if (string.IsNullOrEmpty(value))
                return Array.Empty<string>();

            // Filter nicknames, excluding those already in the table
            var alreadyAddedNicknames = ComplainData.InvolvedUsers.Select(u => u.Nickname).ToHashSet();

            return Nicknames
                .Where(x => !alreadyAddedNicknames.Contains(x))
                .Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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
                InvolvedUser.InvolvedUserRole.Accuser => Color.Error,
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
            if (string.IsNullOrWhiteSpace(SelectedNickname) ||
                SelectedContext == null)
                return;

            // Check if the user is already on the list
            var existingUser = ComplainData.InvolvedUsers.FirstOrDefault(u =>
                u.Nickname == SelectedNickname); //Removed context check for user

            if (existingUser != null)
            {
                // If the user already exists and is "Accused" and a rule is selected,
                // add it to the existing list of rules
                if (SelectedUserRole == InvolvedUser.InvolvedUserRole.Accused &&
                    !string.IsNullOrEmpty(SelectedRule) &&
                    !existingUser.ViolatedRules.Contains(SelectedRule))
                {
                    existingUser.ViolatedRules += (string.IsNullOrEmpty(existingUser.ViolatedRules) ? "" : ", ") +
                                                 SelectedRule;
                }
                return;
            }

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

        public Complaint GetComplainData() => ComplainData;
    }
}

