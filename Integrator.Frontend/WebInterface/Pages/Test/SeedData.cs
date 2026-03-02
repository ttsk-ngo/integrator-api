using Integrator.DataAccess.Models.Users;

namespace Integrator.Frontend.WebInterface.Pages.Test;

public static class SeedData
{
    public class SeedUser : IntegratorUser
    {
        public string Password { get; set; }
        public string Role { get; set; }
    }
    
    public static List<SeedUser> Users = new List<SeedUser>()
    {
        new()
        {
            UserName = "inasmuch",
            Email = "Jonathon66@gmail.com",
            Password = "XCELs6Gk",
            Role = "Admin"
        },
        new()
        {
            UserName = "upon",
            Email = "Althea.Marquardt69@gmail.com",
            Password = "uFL7*ZA&",
            Role = "Moderator"
        },
        new()
        {
            UserName = "major",
            Email = "Arlene11@gmail.com",
            Password = "q^5jUrXp",
            Role = "Moderator"
        },
        new()
        {
            UserName = "dowse",
            Email = "Marshall22@gmail.com",
            Password = "7zX2Aja^",
            Role = "User"
        },
        new()
        {
            UserName = "shoehorn",
            Email = "Hertha62@gmail.com",
            Password = "B$fxsbMV",
            Role = "User"
        }
    };
}