namespace PrescriberPoint.Journal.Persistence.Pgsql.Scripts;

public static class UserScripts
{
    public const string Select = $@"SELECT id, name, username, salt, password_hash FROM users;";
    public const string SelectLimited = $@"SELECT id, name, username FROM users;";
    public const string SelectById = $@"SELECT id, name, username, salt, password_hash FROM users WHERE id = @id;";
    public const string SelectByUsername = "SELECT id, name, username, salt, password_hash FROM users WHERE username = @username;";
    public const string Insert = @"INSERT INTO users (name, username, salt, password_hash) 
                        VALUES (@name, @username, @salt, @password_hash) returning id;";

    public const string Update = @"UPDATE users SET name = @name WHERE id = @id;";

    public const string Delete = @"DELETE FROM users WHERE id = @id;";

    
    public static class Table
    {
        public const string Id = "id";
        public const string Name = "name";
        public const string Username = "username";
        public const string Salt = "salt";
        public const string PasswordHash = "password_hash";
    }
}