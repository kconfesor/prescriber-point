
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrescriberPoint.Journal.Domain;

public interface ITokenService {
    string CreateToken(User user);
}

public class TokenService: ITokenService
{
    // Specify how long until the token expires
    private const int ExpirationMinutes = 30;
    private readonly ILogger<TokenService> _logger;
    private readonly JwtTokenSettings _jwtTokenSettings;

    public TokenService(ILogger<TokenService> logger, IOptions<JwtTokenSettings> jwtTokenSettings)
    {
        _logger = logger;
        _jwtTokenSettings = jwtTokenSettings.Value; 
    }

    public string CreateToken(User user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        
        _logger.LogInformation("JWT Token created");
        
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            _jwtTokenSettings.ValidIssuer,
            _jwtTokenSettings.ValidAudience,
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private List<Claim> CreateClaims(User user)
    {
        var jwtSub =  _jwtTokenSettings.JwtRegisteredClaimNamesSub;

        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSub),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(CustomClaimTypes.UserId, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
                
            };
            
            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var symmetricSecurityKey = _jwtTokenSettings.SymmetricSecurityKey;
        
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }
}