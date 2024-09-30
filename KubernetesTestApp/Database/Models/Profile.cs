namespace KubernetesTestApp.Database.Models;

public sealed class Profile
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Profession { get; init; }
}