using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;

namespace PrescriberPoint.Journal.WebApi.Tests.Infrastructure;

public abstract class BaseWebApplication(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    protected WebApplicationFactory<Program> Factory;

    public async Task InitializeAsync()
    {
        await using var connection = new NpgsqlConnection(fixture.ConnectionString);
        await connection.OpenAsync();

        var command = new NpgsqlCommand(""""
                                        DO $$
                                        BEGIN
                                        CREATE TABLE IF NOT EXISTS users (
                                                                             id SERIAL PRIMARY KEY,
                                                                             name VARCHAR(255),
                                                                             username VARCHAR(255) UNIQUE,
                                                                             salt BYTEA,
                                                                             password_hash VARCHAR(255)
                                        );

                                        CREATE TABLE IF NOT EXISTS journal (
                                                                               id SERIAL PRIMARY KEY,
                                                                               user_id INT NOT NULL,
                                                                               patient VARCHAR(255),
                                                                               note TEXT,
                                                                               created_at TIMESTAMPTZ NOT NULL,
                                                                               modified_at TIMESTAMPTZ,
                                                                               FOREIGN KEY (user_id) REFERENCES users (id)
                                        );
                                        END $$;
                                        """", connection);

        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();

        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(host =>
            {
                host.UseSetting(
                    "ConnectionStrings:journalDb",
                    fixture.ConnectionString
                );
            });
    }

    public async Task DisposeAsync()
    {
        await Task.CompletedTask;
    }
}