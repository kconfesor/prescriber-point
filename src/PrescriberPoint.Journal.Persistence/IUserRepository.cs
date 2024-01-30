using PrescriberPoint.Journal.Domain;

namespace PrescriberPoint.Journal.Persistence;

public interface IUserRepository
{
    public Task<User?> GetById(int userId);
    public Task<User?> GetByUsername(string username);
    public Task<IEnumerable<User>> GetAll();
    public Task Create(User user);
    public Task Update(User user);
    public Task Delete(int userId);
}
