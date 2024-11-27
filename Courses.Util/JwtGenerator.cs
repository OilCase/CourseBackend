using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Courses.Util;

public interface IJwtGenerator
{
    public string CreateToken(string userName, string[]? roles = null, string? modelCase = null, string? userId = null);
}

public class JwtGenerator : IJwtGenerator
{
    private static string _tokenKey;

    public JwtGenerator(IConfiguration config)
    {
        _tokenKey = config["JwtSecretKey"]!;
    }


    public string CreateToken(string userName, string[]? roles = null, string? modelCase = null, string? userId = null)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, userName),
            new Claim(ClaimTypes.Name, userName)
        };

        if ((roles ?? Array.Empty<string>()).Any())
            claims.AddRange(roles!.Select(role => new Claim(ClaimTypes.Role, role)));

        if (!string.IsNullOrEmpty(userId))
        {
            claims.Add(new Claim("UserId", userId));
        }

        var credentials = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays(365),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static SymmetricSecurityKey GetSymmetricSecurityKey(string token) => new(Encoding.UTF8.GetBytes(token));
    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(_tokenKey));

    public static ClaimsPrincipal ValidateToken(string jwtToken)
    {
        IdentityModelEventSource.ShowPII = true;
        ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, GetTokenParams(_tokenKey), out _);
        return principal;
    }

    public static TokenValidationParameters GetTokenParams(string token)
    {
        return new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = GetSymmetricSecurityKey(token),
            ValidateIssuerSigningKey = false
        };
    }
}