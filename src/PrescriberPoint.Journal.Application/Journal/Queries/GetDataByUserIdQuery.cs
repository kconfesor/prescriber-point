using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Journal.Queries;

public interface IGetDataByUserIdQuery
{
    Task<IEnumerable<Domain.Journal>> Handle(int userId);
}

public class GetDataByUserIdQueryHandler: IGetDataByUserIdQuery {
    private readonly IJournalRepository _journalRepository;

    public GetDataByUserIdQueryHandler(IJournalRepository journalRepository) {
        _journalRepository = journalRepository;
    }

    public Task<IEnumerable<Domain.Journal>> Handle(int userId) => _journalRepository.GetByUserId(userId);
}
