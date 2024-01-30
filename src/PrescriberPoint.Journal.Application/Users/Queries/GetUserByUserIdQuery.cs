using PrescriberPoint.Journal.Domain;
using PrescriberPoint.Journal.Persistence;

namespace PrescriberPoint.Journal.Application.Users.Queries;

public interface IGetUserByUserIdQuery
{
    Task<User?> Handle(int userId);
}

public class GetUserByUserIdQuery: IGetUserByUserIdQuery {
    private readonly IUserRepository _userRepository;

    public GetUserByUserIdQuery(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public Task<User?> Handle(int userId) {
        return _userRepository.GetById(userId);
    }
}
