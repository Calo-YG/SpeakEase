using System.Security.Claims;

namespace SpeakEase.Infrastructure.Authorization
{
    /// <summary>
    /// token 管理
    /// </summary>
    public interface ITokenManager
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();
    }
}
