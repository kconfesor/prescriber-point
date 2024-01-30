

namespace PrescriberPoint.Journal.WebApi.Journal.Models;

public record JournalResponse (int JournalId, int UserId, string Patient, string Note, DateTimeOffset CreatedAt, DateTimeOffset? ModifiedAt);