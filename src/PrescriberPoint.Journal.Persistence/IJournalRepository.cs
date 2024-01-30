using PrescriberPoint.Journal.Domain;

namespace PrescriberPoint.Journal.Persistence;

public interface IJournalRepository
{
    public Task<IEnumerable<PrescriberPoint.Journal.Domain.Journal>> GetByUserId(int userId);
    public Task<PrescriberPoint.Journal.Domain.Journal?> GetById(int journalId);
    public Task Create(PrescriberPoint.Journal.Domain.Journal journal);
    public Task<Domain.Journal?> Update(PrescriberPoint.Journal.Domain.Journal journal);
    public Task Delete(int journalId);
}
