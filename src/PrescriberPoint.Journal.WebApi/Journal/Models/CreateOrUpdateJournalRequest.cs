

namespace PrescriberPoint.Journal.WebApi.Journal.Models;

public record CreateOrUpdateJournalRequest (
    string Patient, 
    string Note);