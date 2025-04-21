using System.Security.Claims;

namespace SpeakEase.Infrastructure.Authorization
{
    /// <summary>
    /// token 管理
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// 生成刷新token
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();

        /// <summary>
        /// 解析token
        /// </summary>
        /// <returns></returns>
        bool ValidateTokenExpired();
    }
}
