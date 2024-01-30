namespace PrescriberPoint.Journal.Persistence.Memory;

public class JournalRepository: IJournalRepository
{
    private ICollection<Domain.Journal> _data;
    private int _primaryKeyIdentity = 0;

    public JournalRepository(ICollection<Domain.Journal> data) {
        _data = data;
    }

    public Task<Journal?> GetById(int journalId) 
    {
        var data = _data.FirstOrDefault(x => x.UserId == journalId);
        return Task.FromResult(data);
    }
    public Task<IEnumerable<Journal>> GetByUserId(int userId) 
    {
        var data = _data.Where(d=>d.UserId == userId);
        return Task.FromResult(data);
    }

    public Task Create(Journal journal) {
        journal.Id =  ++_primaryKeyIdentity;
        _data.Add(journal);
        return Task.CompletedTask;
    }

    public Task<Journal?> Update(Journal journal) {
        var dbData = _data.FirstOrDefault(x => x.Id == journal.Id);

        if (dbData == null) {
            return Task.FromResult(dbData);
        }

        dbData.Patient = journal.Patient;
        dbData.Note = journal.Note;
        
        return Task.FromResult<Journal?>(dbData);
    }

    public Task Delete(int journalId) {
        var dbData = _data.FirstOrDefault(x => x.Id == journalId);

        if (dbData == null) {
            return Task.FromResult(dbData);
        }

        _data.Remove(dbData);
        return Task.FromResult<Journal?>(dbData);
    }
}
