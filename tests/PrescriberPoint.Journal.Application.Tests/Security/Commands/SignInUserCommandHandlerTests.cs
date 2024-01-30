using Moq;
using PrescriberPoint.Journal.Application.Security.Commands;
using PrescriberPoint.Journal.Domain;
using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Tests.Security.Commands;

public class SignInUserCommandHandlerTests
{
    private readonly SignInUserCommandHandler _commandHandler;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHashService> _passwordHashServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;

    private readonly User _testUser;
    private const string _password = "Password";

    public SignInUserCommandHandlerTests() {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHashServiceMock = new Mock<IPasswordHashService>();
        _tokenServiceMock = new Mock<ITokenService>();

        var passwordHashService = new PasswordHashService();

        var salt = passwordHashService.GenerateSalt();
        var hash = passwordHashService.GetHash(_password, salt);

       _testUser  = new User{
            Id = 1,
            Name = "Test",
            Username = "Test",
            PasswordHash =  Convert.ToBase64String(hash),
            Salt = salt
        };

        _userRepositoryMock
            .Setup(x=>x.GetByUsername(_testUser.Username))
            .Returns(Task.FromResult<User?>(_testUser));

        _passwordHashServiceMock
            .Setup(x => x.GetHash(_password, salt))
            .Returns(hash);

        _commandHandler = new SignInUserCommandHandler(
            _userRepositoryMock.Object,
            _passwordHashServiceMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    
    public async Task Handle_WithInvalid_Username_Throws_InvalidCredentialException()
    {
        await Assert.ThrowsAsync<InvalidCredentialException>(
            ()=>
            _commandHandler.Handle(new Credential(
                "invalidusername",
                "b"
            )));
    }

    [Fact]
    public async Task Handle_ShouldCall_PasswordHashService_GetHash_Method_Once()
    {
        await _commandHandler.Handle(new Credential(
            _testUser.Username,
            _password
        ));

        _passwordHashServiceMock.Verify(x=>x.GetHash(_password, _testUser.Salt), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalid_Password_Throws_InvalidCredentialException()
    {
        await Assert.ThrowsAsync<InvalidCredentialException>(
            ()=>
            _commandHandler.Handle(new Credential(
            _testUser.Username,
            _password+"InvalidCredential"
        )));
    }

    [Fact]
    public async Task Handle_Returns_SignInTokenModel()
    {
        var signInTokenModel = await _commandHandler.Handle(new Credential(
            _testUser.Username,
            _password
        ));

        Assert.NotNull(signInTokenModel);
    }
}