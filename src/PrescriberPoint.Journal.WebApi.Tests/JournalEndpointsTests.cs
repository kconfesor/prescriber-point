using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using PrescriberPoint.Journal.WebApi.Journal.Models;
using PrescriberPoint.Journal.WebApi.Tests.Infrastructure;
using Xunit.Abstractions;

namespace PrescriberPoint.Journal.WebApi.Tests;

[Collection("Sequential")]
public class JournalEndpointsTests(DatabaseFixture fixture, ITestOutputHelper output) : BaseWebApplication(fixture)
{

    [Fact]
    public async Task Post_Journal_Should_Create()
    {
        await SetupUser();
        var journalRequest = new CreateOrUpdateJournalRequest("Patient one", "Test note");
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
        
        var actual = await client.PostAsJsonAsync("/journal", journalRequest);
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
    }
    
    [Fact]
    public async Task Get_Journal_Should_Return_Users_Journal()
    {
        await SetupUser();
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
        
        var actual = await client.GetFromJsonAsync<List<JournalResponse>>("/journal");
        Assert.NotNull(actual);
    }
    
    
    private async Task SetupUser()
    {
        var signupRequest = new SignUpRequest("johndoe", "John Doe", "password");
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/security/signUp", signupRequest);
    }
    
    private async Task<string> GetToken()
    {
        var signupRequest = new CredentialRequest("johndoe", "password");
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/security/signIn", signupRequest);
        var expected = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return expected?.Token;
    }
}