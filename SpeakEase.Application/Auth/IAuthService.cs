using SpeakEase.Contract.Auth.Dto;
using SpeakEase.Domain.Users.Enum;

namespace SpeakEase.Contract.Auth
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        Task<VerificationCodeResponse> GetVerificationCode(CapchaEnum capcha);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TokenResponse> Login(LoginRequest request);
    }
}
