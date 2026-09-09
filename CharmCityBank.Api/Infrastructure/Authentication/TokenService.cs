using System.Security.Claims;
using System.Text;
using CharmCityBank.Api.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CharmCityBank.Api.Infrastructure.Authentication;

public class TokenService (IOptions<JwtSettings> jwtSetting)
{
    private readonly JwtSettings _jwtSettings = jwtSetting.Value;

    public (string Token, DateTime ExpiresAtUtc) GenerateToken(ApplicationUser user, IEnumerable<string> roles)
    {
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        Console.WriteLine($"UTC Now: {DateTime.UtcNow:O}");
        Console.WriteLine($"Expire Minutes: {_jwtSettings.ExpireMinutes}");
        Console.WriteLine($"Expires: {expiresAtUtc:O}");

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAtUtc,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(descriptor);
        Console.WriteLine($"JWT Token: {token}");
        return (token, expiresAtUtc);
    }
}