using System.Net;
using System.Net.Http.Json;
using PrescriberPoint.Journal.WebApi.Tests.Infrastructure;

namespace PrescriberPoint.Journal.WebApi.Tests;

[Collection(nameof(SecurityEndpointsTests))]
public class SecurityEndpointsTests(DatabaseFixture fixture) : BaseWebApplication(fixture)
{
    [Fact]
    public async Task SignUp_Should_Succeed_And_Create_User()
    {
        var signupRequest = new SignUpRequest("johndoe", "John Doe", "password");
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/security/signUp", signupRequest);
        var expected = await response.Content.ReadFromJsonAsync<SignUpResponse>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("johndoe", expected.Username);
        Assert.True(expected.UserId > 0);
    }
    
    [Fact]
    public async Task SignUp_Should_Return_Bad_Request()
    {
        var signupRequest = new SignUpRequest("", "John Doe", "password");
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/security/signUp", signupRequest);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task SignIn_Should_Succeed_And_Return_Token()
    {
        var signupRequest = new CredentialRequest("johndoe", "password");
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/security/signIn", signupRequest);
        var expected = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(expected);
        Assert.NotEmpty(expected.Token);
    }
}