namespace PrescriberPoint.Journal.Application.Journal.Models;

public record JournalModel(
    int JournalId,
    int UserId,
    string Patient, 
    string Note,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ModifiedAt);