using Npgsql;
using PrescriberPoint.Journal.Domain;
using PrescriberPoint.Journal.Persistence.Pgsql.Scripts;

namespace PrescriberPoint.Journal.Persistence.Pgsql;

public class UserRepository(string connectionString) : IUserRepository
{
    public async Task<User?> GetById(int userId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        await using var selectCommand = new NpgsqlCommand(UserScripts.SelectById, connection);
        selectCommand.Parameters.AddWithValue(UserScripts.Table.Id);

        await using var reader = selectCommand.ExecuteReader();
        if (!reader.Read()) return null;
        
        var user = new User
        {
            Id = reader.GetInt32(reader.GetOrdinal(UserScripts.Table.Id)),
            Name = reader.GetString(reader.GetOrdinal(UserScripts.Table.Name)),
            Username = reader.GetString(reader.GetOrdinal(UserScripts.Table.Username))
        };

        await connection.CloseAsync();
        
        return user;
    }

    public async Task<User?> GetByUsername(string username)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        await using var selectCommand = new NpgsqlCommand(UserScripts.SelectByUsername, connection);
        selectCommand.Parameters.AddWithValue(UserScripts.Table.Username, username);

        await using var reader = selectCommand.ExecuteReader();
        if (!reader.Read()) return null;
        
        var user = new User
        {
            Id = reader.GetInt32(reader.GetOrdinal(UserScripts.Table.Id)),
            Name = reader.GetString(reader.GetOrdinal(UserScripts.Table.Name)),
            Username = reader.GetString(reader.GetOrdinal(UserScripts.Table.Username)),
            Salt = (byte[]) reader[UserScripts.Table.Salt],
            PasswordHash = reader.GetString(reader.GetOrdinal(UserScripts.Table.PasswordHash))
        };

        await connection.CloseAsync();

        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        var users = new List<User>();

        await using var selectCommand = new NpgsqlCommand(UserScripts.SelectLimited, connection);

        await using var reader = selectCommand.ExecuteReader();
        while (reader.Read())
        {
            var user = new User
            {
                Id = reader.GetInt32(reader.GetOrdinal(UserScripts.Table.Id)),
                Name = reader.GetString(reader.GetOrdinal(UserScripts.Table.Name)),
                Username = reader.GetString(reader.GetOrdinal(UserScripts.Table.Username))
            };
            
            users.Add(user);
        }

        await connection.CloseAsync();
        return users;
    }

    public async Task Create(User user)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
        await using var insertCommand = new NpgsqlCommand(UserScripts.Insert, connection);
        
        insertCommand.Parameters.AddWithValue(UserScripts.Table.Name, user.Name);
        insertCommand.Parameters.AddWithValue(UserScripts.Table.Username, user.Username);
        insertCommand.Parameters.AddWithValue(UserScripts.Table.Salt, user.Salt);
        insertCommand.Parameters.AddWithValue(UserScripts.Table.PasswordHash, user.PasswordHash);
        user.Id = (int) (await insertCommand.ExecuteScalarAsync())!;
        
        await connection.CloseAsync();
    }

    public async Task Update(User user)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
        await using var updateCommand = new NpgsqlCommand(UserScripts.Update, connection);
        updateCommand.Parameters.AddWithValue(UserScripts.Table.Id, user.Id);
        updateCommand.Parameters.AddWithValue(UserScripts.Table.Name, user.Name);

        await updateCommand.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }

    public async Task Delete(int userId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
        await using var deleteCommand = new NpgsqlCommand(UserScripts.Delete, connection);
        deleteCommand.Parameters.AddWithValue(UserScripts.Table.Id, userId);

        await deleteCommand.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }
}