using System.Security.Claims;
using System.Text.RegularExpressions;
using IdGen;
using Lazy.Captcha.Core;
using Microsoft.EntityFrameworkCore;
using SpeakEase.Contract.Auth;
using SpeakEase.Contract.Auth.Dto;
using SpeakEase.Domain.Users;
using SpeakEase.Domain.Users.Const;
using SpeakEase.Infrastructure.Authorization;
using SpeakEase.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Redis;
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
    /// <param name="tokenManager">token生成器</param>
    public class AuthService(IDbContext context,ICaptcha captcha,IdGenerator idgenerator,ITokenManager tokenManager,IRedisService redisService) :IAuthService
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [EndpointSummary("获取验证码")]
        public async Task<VerificationCodeResponse> GetVerificationCode(string capcha)
        {
            var id = idgenerator.CreateId();

            var key = LongToStringConverter.Convert(id);

            var capchakey = capcha switch
            {
                "Login" => string.Format(UserConst.LoginCapcahCache, key),
                "Register" => string.Format(UserConst.RegiesCapchaCache, key),
                "Modify" => string.Format(UserConst.ModifyUserCapchaCache, key),
                _ => throw new UserFriednlyException("参数错误")
            };

            var code = captcha.Generate(LongToStringConverter.Convert(id),240);

            await redisService.SetAsync(capchakey, code.Code,600);

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
        [EndpointSummary("登录")]
        public async Task<TokenResponse> Login(LoginRequest request)
        {
            if (request.Password.IsNullOrEmpty()
                    || request.Password.Length < 6 ||
                    !Regex.IsMatch(request.Password, @"^(?=.*[0-9])(?=.*[a-zA-Z]).*$"))
            {
                ThrowUserFriendlyException.ThrowException("密码长度至少6位，且必须包含字母和数字");
            }

            if (request.UserAccount.IsNullOrEmpty())
            {
                ThrowUserFriendlyException.ThrowException("请输入账号");
            }

            if (request.UniqueId.IsNullOrEmpty() || request.Code.IsNullOrEmpty())
            {
                ThrowUserFriendlyException.ThrowException("请输入验证码");
            }

            var key = string.Format(UserConst.LoginCapcahCache, request.UniqueId);

            var code = redisService.Get(key);

            var validate = captcha.Validate(request.UniqueId, request.Code);

            if (request.Code != code)
            {
                ThrowUserFriendlyException.ThrowException("验证码校验错误");
            }

            var user = await context.User.AsNoTracking().FirstOrDefaultAsync(p=>p.UserAccount == request.UserAccount);

            if(user is null)
            {
                ThrowUserFriendlyException.ThrowException("用户不存在");
            }

            var checkpassword = BCrypt.Net.BCrypt.Verify(request.Password, user.UserPassword);

            if (!checkpassword)
            {
                ThrowUserFriendlyException.ThrowException("密码错误");
            }

            var clamis = new List<Claim>() 
            {
                new Claim(type:UserInfomationConst.UserName,user.UserName),
                new Claim(type:UserInfomationConst.UserAccount,user.UserAccount),
                new Claim(type:UserInfomationConst.OrganizationName,string.Empty),
                new Claim(type:UserInfomationConst.UserId,LongToStringConverter.Convert(user.Id)),
                //new Claim
            };

            var toekn = tokenManager.GenerateAccessToken(clamis);
            var refreshToken = tokenManager.GenerateRefreshToken();

            var entity = new RefreshTokenEntity(idgenerator.CreateId(),
                user.Id,
                refreshToken,
                DateTime.Now.AddMinutes(10),
                false);

            context.RefreshToken.Add(entity);

            await context.SaveChangesAsync();

            await redisService.DeleteAsync(key);

            return new TokenResponse
            {
                Token = toekn,
                RefreshToken = refreshToken,
            };
        }
    }
}
