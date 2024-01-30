



using PrescriberPoint.Journal.Application.Common.Exceptions;

public class InvalidCredentialException: BadRequestException {

    public InvalidCredentialException() 
    : base("Invalid username or passowrd"){}
}