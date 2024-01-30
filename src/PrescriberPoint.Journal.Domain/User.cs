namespace PrescriberPoint.Journal.Domain;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public byte[] Salt { get; set; } = [];
    public string PasswordHash { get; set; } = string.Empty;
}
