using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
public class TokenManager(IOptionsSnapshot<JwtOptions> options,IHttpContextAccessor httpContextAccessor,ILoggerFactory loggerFactory): ITokenManager
{
    /// <summary>
    /// 获取jwtoptions
    /// </summary>
    private readonly JwtOptions option = options.Value;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger logger = loggerFactory.CreateLogger("TokenManager");

    /// <summary>
    /// Token Handler
    /// </summary>
    private readonly JwtSecurityTokenHandler SecurityTokenHandler = new JwtSecurityTokenHandler();

    /// <summary>
    /// 生成token
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        if (string.IsNullOrEmpty(option.SecretKey) || string.IsNullOrEmpty(option.Issuer) || string.IsNullOrEmpty(option.Issuer))
        {
            ThrowUserFriendlyException.ThrowException("validate jwt options failed");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(option.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: option.Issuer,
            audience: option.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(option.ExpMinutes), // 短有效期
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// 生成refreshtoken
    /// </summary>
    /// <returns></returns>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// token
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public JwtSecurityToken GetSecurityToken()
    {
        var token = GetToken();

        return SecurityTokenHandler.ReadJwtToken(token);
    }

    private string GetToken()
    {
        var tryget = httpContextAccessor.HttpContext.Request.Headers.TryGetValue(UserInfomationConst.AuthorizationHeader, out var token);

        if (!tryget)
        {
            ThrowUserFriendlyException.ThrowException("can not get token value");
        }

        return token.ToString().Replace(UserInfomationConst.TokenPrefix,"");
    }

    /// <summary>
    /// 解析token
    /// </summary>
    /// <returns></returns>
    public bool ValidateTokenExpired()
    {
        var token = GetToken();

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
            principal = SecurityTokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        }
        catch (Exception ex)
        {
            return ex is SecurityTokenExpiredException;
        }

        return true; 
    }
}