namespace Integrator.DataAccess.Models.Editor
{
    public class Tag
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public override string ToString() => Name;
    }
}
