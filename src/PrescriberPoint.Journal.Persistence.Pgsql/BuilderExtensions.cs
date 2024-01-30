using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PrescriberPoint.Journal.Persistence.Pgsql;

public static class BuilderExtensions
{
    public static void AddPostgresPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("JournalDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(connectionString);
        }
        
        services.AddScoped<IUserRepository>(opt => new UserRepository(connectionString));
        services.AddScoped<IJournalRepository>(opt => new JournalRepository(connectionString));
    }
}