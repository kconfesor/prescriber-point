using Npgsql;
using PrescriberPoint.Journal.Domain;
using PrescriberPoint.Journal.Persistence.Pgsql.Scripts;

namespace PrescriberPoint.Journal.Persistence.Pgsql;

public class JournalRepository(string connectionString) : IJournalRepository
{
    public async Task<IEnumerable<Domain.Journal>> GetByUserId(int userId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        await using var selectCommand = new NpgsqlCommand(JournalScripts.SelectByUserId, connection);
        selectCommand.Parameters.AddWithValue(JournalScripts.Table.UserId, userId);
        await using var reader = selectCommand.ExecuteReader();
        
        var journals = new List<Domain.Journal>();
        while (reader.Read())
        {
            var journal = Get(reader);
            journals.Add(journal);
        }
        
        await connection.CloseAsync();
        
        return journals;
    }

    public async Task<Domain.Journal?> GetById(int journalId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        await using var selectCommand = new NpgsqlCommand(JournalScripts.SelectById, connection);
        selectCommand.Parameters.AddWithValue(JournalScripts.Table.Id);

        await using var reader = selectCommand.ExecuteReader();
        if (!reader.Read()) return null;
        
        var journal = Get(reader);
        await connection.CloseAsync();
        
        return journal;
    }


    public async Task Create(Domain.Journal journal)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
        await using var insertCommand = new NpgsqlCommand(JournalScripts.Insert, connection);
        
        insertCommand.Parameters.AddWithValue(JournalScripts.Table.UserId, journal.UserId);
        insertCommand.Parameters.AddWithValue(JournalScripts.Table.Patient, journal.Patient);
        insertCommand.Parameters.AddWithValue(JournalScripts.Table.Note, journal.Note);
        insertCommand.Parameters.AddWithValue(JournalScripts.Table.CreatedAt, journal.CreatedAt);
        journal.Id = (int) (await insertCommand.ExecuteScalarAsync())!;
        
        await connection.CloseAsync();
    }

    public async Task<Domain.Journal?> Update(Domain.Journal journal)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
        await using var updateCommand = new NpgsqlCommand(JournalScripts.Update, connection);
        updateCommand.Parameters.AddWithValue(JournalScripts.Table.Patient, journal.Patient);
        updateCommand.Parameters.AddWithValue(JournalScripts.Table.Note, journal.Note);
        updateCommand.Parameters.AddWithValue(JournalScripts.Table.ModifiedAt, journal.ModifiedAt);

        await updateCommand.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        
        return journal;
    }

    public async Task Delete(int journalId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
        await using var deleteCommand = new NpgsqlCommand(JournalScripts.Delete, connection);
        deleteCommand.Parameters.AddWithValue(JournalScripts.Table.Id, journalId);

        await deleteCommand.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }
    
    private static Domain.Journal Get(NpgsqlDataReader reader)
    {
        var modifiedAt = reader.GetOrdinal(JournalScripts.Table.ModifiedAt);
        
        var journal = new Domain.Journal()
        {
            Id = reader.GetInt32(reader.GetOrdinal(JournalScripts.Table.Id)),
            UserId = reader.GetInt32(reader.GetOrdinal(JournalScripts.Table.UserId)),
            Patient = reader.GetString(reader.GetOrdinal(JournalScripts.Table.Patient)),
            Note = reader.GetString(reader.GetOrdinal(JournalScripts.Table.Note)),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal(JournalScripts.Table.CreatedAt)),
            ModifiedAt = reader.IsDBNull(modifiedAt) ? (DateTimeOffset?) null : (DateTimeOffset?)reader.GetDateTime(modifiedAt)
        };
        
        return journal;
    }
}