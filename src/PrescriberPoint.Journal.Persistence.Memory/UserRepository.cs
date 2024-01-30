using TechnicalInterview.Domain;
using PrescriberPoint.Journal.Persistence;

namespace TechnicalInterview.Infrastructure.Users.Persistences.Memory;

public class UserRepository: IUserRepository
{
    private ICollection<User> _users;
    private int _primaryKeyIdentity = 0;

    public UserRepository(ICollection<User> users) {
        _users = users;
    }

    public Task<User?> GetById(int userId) 
    {
        var user = _users.FirstOrDefault(x => x.Id == userId);
        return Task.FromResult(user);
    }
    public Task<IEnumerable<User>> GetAll() 
    {
        return Task.FromResult(_users.AsEnumerable());
    }
    public Task<User?> GetByUsername(string username) 
    {
        var user = _users.FirstOrDefault(x => x.Username == username);
        return Task.FromResult(user);
    }

    public Task Create(User user) {
        user.Id =  ++_primaryKeyIdentity;
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task Update(User user) {
        var dbUser = _users.FirstOrDefault(x => x.Id == user.Id);

        if (dbUser == null) {
            return Task.CompletedTask;
        }

        dbUser.Name = user.Name;
        return Task.CompletedTask;
    
    }

    public Task Delete(int userId) {
        var dbUser = _users.FirstOrDefault(x => x.Id == userId);

        if (dbUser == null) {
            return Task.CompletedTask;
        }

        _users.Remove(dbUser);
        return Task.CompletedTask;
    }
}
