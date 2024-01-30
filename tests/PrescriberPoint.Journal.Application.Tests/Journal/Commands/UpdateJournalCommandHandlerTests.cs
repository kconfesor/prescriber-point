
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using PrescriberPoint.Journal.Application.Journal.Commands;
using PrescriberPoint.Journal.Domain;
using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Tests.Journal.Commands;

public class UpdateJournalCommandHandlerTests
{
    private readonly UpdateDataCommandHandler _commandHandler;
    private readonly Mock<IJournalRepository> _journalRepositoryMock;
    private readonly Mock<IValidator<Domain.Journal>> _journalValidatorMock;

    public UpdateJournalCommandHandlerTests() {
        _journalRepositoryMock = new Mock<IJournalRepository>();
        _journalValidatorMock = new Mock<IValidator<Domain.Journal>>();
        _commandHandler = new UpdateDataCommandHandler(
            _journalRepositoryMock.Object,
            _journalValidatorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCall_JournalRepository_Update_Method_Once()
    {
        await _commandHandler.Handle(new UpdateDataCommandParameters(
            1,
            "Test",
            "Test"
        ));

        _journalRepositoryMock.Verify(x=>x.Update(It.IsAny<Domain.Journal>()), Times.Once);
    }
}