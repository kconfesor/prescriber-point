// using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TechnicalInterview.Infrastructure.Users.Persistences.Memory;
using PrescriberPoint.Journal.Persistence;

namespace TechnicalInterview.Infrastructure;

public static class BuilderExtensions
{
    // public static void AddMemoryPersistence(this WebApplicationBuilder builder) {
    //     builder.Services.AddSingleton<IJournalRepository>(new JournalRepository(new List<Domain.Journal>()));
    //     builder.Services.AddSingleton<IUserRepository>(new UserRepository(new List<Domain.User>()));
    // }
}
