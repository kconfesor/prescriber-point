using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Security.Commands;

public record Credential(string Username, string Password);


public interface ISignInUserCommand {
    Task<SignInTokenModel?> Handle(Credential parameters);
}

public class SignInUserCommandHandler : ISignInUserCommand
{
    private IUserRepository _userRepository;
    private IPasswordHashService _passwordHashService;
    private ITokenService _tokenService;

    public SignInUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHashService passwordHashService,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _tokenService = tokenService;
    }

    public async Task<SignInTokenModel?> Handle(Credential parameters)
    {
        var user = await _userRepository.GetByUsername(parameters.Username);
        if (user == null) {
            throw new InvalidCredentialException();
        }

        var hash = Convert.ToBase64String(_passwordHashService.GetHash(parameters.Password, user.Salt));

        if (hash != user.PasswordHash) {
            throw new InvalidCredentialException();
        }

        return new SignInTokenModel {
            Token = _tokenService.CreateToken(user)
        };
    }
}