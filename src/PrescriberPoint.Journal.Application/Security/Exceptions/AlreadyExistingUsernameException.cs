using PrescriberPoint.Journal.Application.Common.Exceptions;

namespace PrescriberPoint.Journal.Application.Security.Exceptions;

public class AlreadyExistingUsernameException : BadRequestException {

    public AlreadyExistingUsernameException(string username)
        : base($"The Username '{username}' already exists.") {}

}