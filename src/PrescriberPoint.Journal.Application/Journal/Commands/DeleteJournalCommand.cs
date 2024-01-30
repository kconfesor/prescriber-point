using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Journal.Commands;

public record DeleteJournalCommandParameters(int DataId);

public interface IDeleteDataCommand
{
    Task Handle(DeleteJournalCommandParameters parameters);
}

public class DeleteDataCommandHandler: IDeleteDataCommand {
    private readonly IJournalRepository _journalRepository;

    public DeleteDataCommandHandler(IJournalRepository journalRepository) {
        _journalRepository = journalRepository;
    }

    public async Task Handle(DeleteJournalCommandParameters parameters)
    {
        await _journalRepository.Delete(parameters.DataId);
    }
}
