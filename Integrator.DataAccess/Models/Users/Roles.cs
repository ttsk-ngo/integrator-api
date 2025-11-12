using System.ComponentModel.DataAnnotations;

namespace Integrator.DataAccess.Models.Users
{
    public enum Roles
    {
        [Display(Name = "User")]
        User,

        [Display(Name = "Deserved")]
        Deserved,

        [Display(Name = "Sponsor")]
        Sponsor,

        [Display(Name = "Veteran")]
        Veteran,

        [Display(Name = "Constructor")]
        Constructor,

        [Display(Name = "Head of Constructors")]
        HeadOfConstructors,

        [Display(Name = "Editor")]
        Editor,

        [Display(Name = "Head of Editor")]
        HeadOfEditor,

        [Display(Name = "Coach")]
        Coach,

        [Display(Name = "Head of Coach")]
        HeadOfCoach,

        [Display(Name = "TTSK Archives")]
        TTSKArchives,

        [Display(Name = "Developer")]
        Developer,

        [Display(Name = "Head of Developers")]
        HeadOfDevelopers,

        [Display(Name = "Moderator")]
        Moderator,

        [Display(Name = "Head of Moderators")]
        HeadOfModerators,

        [Display(Name = "Administrator")]
        Administrator,

        [Display(Name = "Head of Administrators")]
        HeadOfAdministrators,

        [Display(Name = "Management")]
        Management,
    }
}
