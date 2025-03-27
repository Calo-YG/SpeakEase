using SpeakEase.Contract.Auth.Dto;

namespace SpeakEase.Contract.Auth
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public interface IAuthService
    {
        Task<VerificationCodeResponse> GetVerificationCode();
    }
}
