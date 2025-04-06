using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Filters;

namespace SpeakEase.Infrastructure.Authorization;

/// <summary>
/// token 管理
/// </summary>
/// <param name="options"></param>
/// <param name="httpContextAccessor"></param>
public class TokenManager(IOptionsSnapshot<JwtOptions> options,IHttpContextAccessor httpContextAccessor): ITokenManager
{
    private readonly JwtOptions option = options.Value;
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secret = option.SecretKey;
        var issuser = option.Issuer;
        var audience = option.Audience;

        var expire = option.ExpMinutes;

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

    /// redis toekn 校验
    //public Task CheckToken()
    //{

    //}

    /// <summary>
    /// 解析token
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Claim> GetClaims()
    {
        var tryget = httpContextAccessor.HttpContext.Request.Headers.TryGetValue(UserInfomationConst.AuthorizationHeader, out var token);

        if(!tryget)
        {
            ThrowUserFriendlyException.ThrowException("can not get token value");
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = option.Issuer, // 替换为实际的发行者
            ValidAudience = option.Audience, // 替换为实际的受众
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(option.SecretKey)) // 替换为实际的密钥
        };

        ClaimsPrincipal principal = null;

        try
        {
            principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        }
        catch (Exception)
        {
            // 处理验证失败的情况
            ThrowUserFriendlyException.ThrowException("token validate failed");
        }

        return principal?.Claims;
    }
}