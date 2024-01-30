namespace PrescriberPoint.Journal.Persistence.Pgsql.Scripts;

public static class JournalScripts
{
    public const string Select = @"SELECT id, user_id, patient, note, created_at, modified_at FROM journal;";
    public const string SelectById = @"SELECT id, user_id, patient, note, created_at, modified_at FROM journal WHERE id = @id;";
    public const string SelectByUserId = @"SELECT id, user_id, patient, note, created_at, modified_at FROM 
                                                               journal WHERE user_id = @user_id;";
    
    
    public const string Insert = @"INSERT INTO journal (user_id, patient, note, created_at) 
                        VALUES (@user_id, @patient, @note, @created_at) returning id;";

    public const string Update = @"UPDATE journal SET patient = @patient, note = @note, modified_at = @modified_at
               WHERE id = @id;";

    public const string Delete = @"DELETE FROM journal WHERE id = @id;";

    
    public static class Table
    {
        public const string Id = "id";
        public const string UserId = "user_id";
        public const string Patient = "patient";
        public const string Note = "note";
        public const string CreatedAt = "created_at";
        public const string ModifiedAt = "modified_at";
    }
}