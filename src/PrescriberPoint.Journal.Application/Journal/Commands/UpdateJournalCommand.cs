using FluentValidation;
using PrescriberPoint.Journal.Application.Journal.Models;
using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Journal.Commands;

public record UpdateDataCommandParameters (
    int JournalId,
    string Patient,
    string Note
);

public interface IUpdateDataCommand
{
    Task<JournalModel?> Handle(UpdateDataCommandParameters parameters);
}

public class UpdateDataCommandHandler: IUpdateDataCommand {
    private readonly IJournalRepository _journalRepository;
    private  IValidator<Domain.Journal> _validator;

    public UpdateDataCommandHandler(
        IJournalRepository journalRepository,
        IValidator<Domain.Journal> validator) {
        _journalRepository = journalRepository;
        _validator = validator;
    }

    public async Task<JournalModel?> Handle(UpdateDataCommandParameters parameters) {
        var data = new Domain.Journal{
            Id = parameters.JournalId,
            Note = parameters.Note,
            Patient = parameters.Patient,
            ModifiedAt = DateTimeOffset.UtcNow
        };

        await _validator.ValidateAndThrowAsync(data);
        
        var journal = await _journalRepository.Update(data);
        if (journal is null) {
            return null;
        }

        return new JournalModel(journal.Id, journal.UserId, journal.Patient, journal.Note, journal.CreatedAt,
            journal.ModifiedAt);
    }
}
