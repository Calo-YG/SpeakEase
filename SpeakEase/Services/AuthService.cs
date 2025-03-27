using FastService;
using IdGen;
using Lazy.Captcha.Core;
using Microsoft.Extensions.Caching.Distributed;
using SpeakEase.Contract.Auth;
using SpeakEase.Contract.Auth.Dto;
using SpeakEase.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Filters;
using SpeakEase.Infrastructure.SpeakEase.Core;

namespace SpeakEase.Services
{
    /// <summary>
    /// 授权服务
    /// </summary>
    /// <param name="context">数据库上下文</param>
    /// <param name="distributedCache">分布式缓存</param>
    /// <param name="captcha">验证码</param>
    /// <param name="idgenerator">id生成器</param>
    [Filter(typeof(ResultEndPointFilter))]
    [Route("api/auth")]
    [Tags("auth-授权服务")]

    public class AuthService(IDbContext context, IDistributedCache distributedCache,ICaptcha captcha,IdGenerator idgenerator) :FastApi,IAuthService
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [EndpointSummary("获取验证码")]
        public async Task<VerificationCodeResponse> GetVerificationCode()
        {
            var id = idgenerator.CreateId();

            var code = captcha.Generate(LongToStringConverter.Convert(id),240);

            await Task.Delay(1);

            return new VerificationCodeResponse
            {
                UniqueId = id,
                VerificationCode = "data:image/png;base64," + code.Base64
            };
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<TokenResponse> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
