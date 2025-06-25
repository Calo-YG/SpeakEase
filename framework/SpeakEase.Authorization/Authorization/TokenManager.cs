using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Options;

namespace SpeakEase.Authorization.Authorization;

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
    private readonly JwtOptions _option = options.Value;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger = loggerFactory.CreateLogger("TokenManager");

    /// <summary>
    /// Token Handler
    /// </summary>
    private readonly JwtSecurityTokenHandler _securityTokenHandler = new JwtSecurityTokenHandler();

    /// <summary>
    /// 生成token
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        if (string.IsNullOrEmpty(_option.SecretKey) || string.IsNullOrEmpty(_option.Issuer) || string.IsNullOrEmpty(_option.Issuer))
        {
            ThrowUserFriendlyException.ThrowException("validate jwt options failed");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_option!.SecretKey!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _option.Issuer,
            audience: _option.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_option.ExpMinutes), // 短有效期
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

        return _securityTokenHandler.ReadJwtToken(token);
    }

    private string GetToken()
    {
        var tryget = httpContextAccessor!.HttpContext!.Request.Headers.TryGetValue(UserInfomationConst.AuthorizationHeader, out var token);

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
            ValidIssuer = _option.Issuer, // 替换为实际的发行者
            ValidAudience = _option.Audience, // 替换为实际的受众
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_option.SecretKey)) // 替换为实际的密钥
        };
        
        try
        {
            _securityTokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        }
        catch (Exception ex)
        {
            return ex is SecurityTokenExpiredException;
        }

        return true; 
    }
}