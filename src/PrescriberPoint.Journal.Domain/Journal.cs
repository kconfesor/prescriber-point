namespace PrescriberPoint.Journal.Domain;

/// <summary>
/// Journal record.
/// </summary>
public class Journal
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Patient { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }
}
