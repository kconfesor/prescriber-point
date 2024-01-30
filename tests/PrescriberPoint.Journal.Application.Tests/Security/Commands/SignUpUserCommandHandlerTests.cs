using FluentValidation;
using Moq;
using PrescriberPoint.Journal.Application.Security.Commands;
using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Tests.Security.Commands;

public class SignUpUserCommandHandlerTests
{
    private readonly SignUpUserCommandHandler _commandHandler;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHashService> _passwordHashServiceMock;
    private readonly Mock<IValidator<SignUpUserParameters>> _validatorMock;

    public SignUpUserCommandHandlerTests() {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHashServiceMock = new Mock<IPasswordHashService>();
        _validatorMock = new Mock<IValidator<SignUpUserParameters>>();
        _commandHandler = new SignUpUserCommandHandler(
            _userRepositoryMock.Object,
            _passwordHashServiceMock.Object,
            _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCall_UserRepository_GetByUsername_Method_Once()
    {
        var username = "Test";
        await _commandHandler.Handle(new SignUpUserParameters(
            username,
            "b",
            "c"
        ));

        _userRepositoryMock.Verify(x=>x.GetByUsername(username), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCall_PasswordHashService_GenerateSalt_Method_Once()
    {
        await _commandHandler.Handle(new SignUpUserParameters(
            "a",
            "b",
            "c"
        ));

        _passwordHashServiceMock.Verify(x=>x.GenerateSalt(It.IsAny<int>()), Times.Once);
    }

  [Fact]
    public async Task Handle_ShouldCall_PasswordHashService_GetHash_Method_Once()
    {
        var password = "Test";
        await _commandHandler.Handle(new SignUpUserParameters(
            "a",
            "b",
            password
        ));

        _passwordHashServiceMock.Verify(x=>x.GetHash(password, It.IsAny<byte[]>()), Times.Once);
    }

}