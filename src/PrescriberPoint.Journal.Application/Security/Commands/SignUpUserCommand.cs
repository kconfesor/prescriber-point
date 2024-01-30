using FluentValidation;
using PrescriberPoint.Journal.Application.Security.Exceptions;
using PrescriberPoint.Journal.Domain;
using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Security.Commands;

public record SignUpUserParameters(string Username, string Name, string Password);


public interface ISignUpUserCommand {
    Task<SignUpResultModel> Handle(SignUpUserParameters parameters);
}

public class SignUpUserCommandHandler : ISignUpUserCommand
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IValidator<SignUpUserParameters> _validator;

    public SignUpUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHashService passwordHashService,
        IValidator<SignUpUserParameters> validator)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _validator = validator;
    }

    public async Task<SignUpResultModel> Handle(SignUpUserParameters parameters)
    {
        await _validator.ValidateAndThrowAsync(parameters);
        var user = await _userRepository.GetByUsername(parameters.Username);
        
        if (user != null) {
            throw new AlreadyExistingUsernameException(parameters.Username);
        }

        var salt = _passwordHashService.GenerateSalt();

        var hash = _passwordHashService.GetHash(parameters.Password, salt);

        user = new User {
            Username = parameters.Username,
            Name = parameters.Name,
            Salt = salt,
            PasswordHash = Convert.ToBase64String(hash)
        };

        await _userRepository.Create(user);

        return new SignUpResultModel(user.Id, user.Username, user.Name);
    }
}