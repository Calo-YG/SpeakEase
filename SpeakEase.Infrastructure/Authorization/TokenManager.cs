using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SpeakEase.Infrastructure.Exceptions;

namespace SpeakEase.Infrastructure.Authorization;

public class TokenManager(IConfiguration configuration)
{
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secret = configuration["App:JwtOptions:SecretKey"];
        var issuser = configuration["App:JwtOptions:Issuer"];
        var audience = configuration["App:JwtOptions:Audience"];

        var expire = configuration.GetSection("App:JwtOptions:ExpMinutes").Get<int?>() ?? 30;

        if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuser) || string.IsNullOrEmpty(audience))
        {
            ThrowUserFriendlyException.ThrowException("validate jwt options failed");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuser,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expire), // 短有效期
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}