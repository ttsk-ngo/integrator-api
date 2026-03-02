namespace Integrator.DataAccess.Models.Editor
{
    public class User
    {
        public int Age => DateTime.Today.Year - DateOfBirth.Year - (DateOfBirth.Date > DateTime.Today.AddYears(-DateOfBirth.Year) ? 1 : 0);
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public override string ToString() => FullName;
    }
}
