using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Journal.Queries;

public interface IGetDataByDataIdQuery
{
    Task<Domain.Journal?> Handle(int dataId);
}

public class GetDataByDataIdQueryHandler: IGetDataByDataIdQuery {
    private readonly IJournalRepository _journalRepository;

    public GetDataByDataIdQueryHandler(IJournalRepository journalRepository) {
        _journalRepository = journalRepository;
    }

    public Task<Domain.Journal?> Handle(int dataId) => _journalRepository.GetById(dataId);
}
