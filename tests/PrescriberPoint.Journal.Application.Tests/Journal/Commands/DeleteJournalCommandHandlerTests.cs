
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using PrescriberPoint.Journal.Application.Journal.Commands;
using PrescriberPoint.Journal.Domain;
using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Tests.Journal.Commands;

public class DeleteJournalCommandHandlerTests
{
    private readonly DeleteDataCommandHandler _commandHandler;
    private readonly Mock<IJournalRepository> _journalRepositoryMock;

    public DeleteJournalCommandHandlerTests() {
        _journalRepositoryMock = new Mock<IJournalRepository>();
        _commandHandler = new DeleteDataCommandHandler(
            _journalRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCall_JournalRepository_Delete_Method_Once()
    {
        await _commandHandler.Handle(new DeleteJournalCommandParameters(
            1
        ));

        _journalRepositoryMock.Verify(x=>x.Delete(It.IsAny<int>()), Times.Once);
    }
}