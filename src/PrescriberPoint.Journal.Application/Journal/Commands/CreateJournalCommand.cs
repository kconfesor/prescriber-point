using FluentValidation;
using PrescriberPoint.Journal.Application.Journal.Models;
using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Journal.Commands;

public record CreateDataCommandParameters(
    int UserId, 
    string Patient,
    string Note
);

public interface ICreateDataCommand
{
    Task<JournalModel> Handle(CreateDataCommandParameters parameters);
}

public class CreateDataCommandHandler: ICreateDataCommand {
    private readonly IJournalRepository _journalRepository;
    private readonly IValidator<Domain.Journal> _validator;

    public CreateDataCommandHandler(
        IJournalRepository journalRepository,
        IValidator<Domain.Journal> validator) 
    {
        _journalRepository = journalRepository;
        _validator = validator;
    }

    public async Task<JournalModel> Handle(CreateDataCommandParameters parameters) {
        var data = new Domain.Journal{
            UserId = parameters.UserId,
            Patient = parameters.Patient,
            Note = parameters.Note,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _validator.ValidateAndThrowAsync(data);
        await _journalRepository.Create(data);
        
        return new JournalModel(data.Id, data.UserId, data.Patient, data.Note, data.CreatedAt, data.ModifiedAt);
    }
}
