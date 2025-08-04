using System.Security.Claims;
using System.Text.RegularExpressions;
using IdGen;
using Lazy.Captcha.Core;
using Microsoft.EntityFrameworkCore;
using SpeakEase.Authorization.Authorization;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Redis;
using SpeakEase.Infrastructure.SpeakEase.Core;
using SpeakEase.Study.Contract.Auth;
using SpeakEase.Study.Contract.Auth.Dto;
using SpeakEase.Study.Domain.Users;
using SpeakEase.Study.Domain.Users.Const;
using SpeakEase.Study.Infrastructure.EntityFrameworkCore;

namespace SpeakEase.Study.Application.Auth
{
    /// <summary>
    /// 授权服务
    /// </summary>
    /// <param name="context">数据库上下文</param>
    /// <param name="captcha">验证码</param>
    /// <param name="idgenerator">id生成器</param>
    /// <param name="tokenManager">token生成器</param>
    /// <param name="redisService">redis缓存</param>
    public class AuthService(IDbContext context, ICaptcha captcha, IdGenerator idgenerator, ITokenManager tokenManager, IRedisService redisService) : IAuthService
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public async Task<VerificationCodeDto> GetVerificationCode(string capcha)
        {
            var id = idgenerator.CreateId();

            var key = LongToStringConverter.Convert(id);

            var capcheKey = capcha switch
            {
                "Login" => string.Format(UserConst.LoginCapcahCache, key),
                "Register" => string.Format(UserConst.RegiesCapchaCache, key),
                "Modify" => string.Format(UserConst.ModifyUserCapchaCache, key),
                _ => throw new UserFriendlyException("参数错误")
            };

            var unique = LongToStringConverter.Convert(id);

            var code = captcha.Generate(LongToStringConverter.Convert(id), 240);

            await redisService.SetAsync(capcheKey, code.Code, 600);

            return new VerificationCodeDto
            {
                UniqueId = key,
                VerificationCode = "data:image/png;base64," + code.Base64
            };
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TokenDto> Login(LoginInput request)
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

            if (request.Code.IsNullOrEmpty())
            {
                ThrowUserFriendlyException.ThrowException("请输入验证码");
            }

            var key = string.Format(UserConst.LoginCapcahCache, request.UniqueId);

            var code = redisService.Get(key);

            if (request.Code != code)
            {
                ThrowUserFriendlyException.ThrowException("验证码校验错误");
            }

            var user = await context.User.AsNoTracking().FirstOrDefaultAsync(p => p.UserAccount == request.UserAccount);

            if (user is null)
            {
                ThrowUserFriendlyException.ThrowException("用户不存在");
            }

            var checkpassword = BCrypt.Net.BCrypt.Verify(request.Password, user!.Password);

            if (!checkpassword)
            {
                ThrowUserFriendlyException.ThrowException("密码错误");
            }

            var clamis = new List<Claim>()
            {
                new Claim(type:UserInfomationConst.UserName,user.UserName),
                new Claim(type:UserInfomationConst.UserAccount,user.UserAccount),
                new Claim(type:UserInfomationConst.UserId,user.Id),
            };

            var token = tokenManager.GenerateAccessToken(clamis);
            var refreshToken = tokenManager.GenerateRefreshToken();

            var entity = new RefreshTokenEntity(idgenerator.CreateId(),
                user.Id,
                refreshToken,
                DateTime.Now.AddMinutes(40),
                false);

            context.RefreshToken.Add(entity);

            await context.SaveChangesAsync();

            await redisService.DeleteAsync(key);

            var res = new TokenDto
            {
                Token = token,
                RefreshToken = refreshToken,
            };

            await redisService.SetAsync(string.Format(UserInfomationConst.RedisTokenKey, user.Id), res, 30 * 1000 * 60);

            return res;
        }

        /// <summary>
        /// 推出登录
        /// </summary>
        /// <returns></returns>
        public async Task LoginOut()
        {
            var user = context.GetUser();

            var key = string.Format(UserInfomationConst.RedisTokenKey, user.Id);

            await redisService.DeleteAsync(key);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        public async Task<RefreshTokenDto> RefreshToken(RefreshTokenInput request)
        {
            if (request.Id.IsNullOrEmpty())
            {
                ThrowUserFriendlyException.ThrowException("请输入随机id");
            }

            if (request.RefresherToken.IsNullOrEmpty())
            {
                ThrowUserFriendlyException.ThrowException("请输入刷新token");
            }

            var entity = await context.QueryNoTracking<RefreshTokenEntity>().FirstOrDefaultAsync(p => p.UserId == request.Id && p.Token == request.RefresherToken);

            if (entity is null)
            {
                ThrowUserFriendlyException.ThrowException("非法RefreshToken");
            }

            if (entity!.ExpireAt <= DateTime.Now)
            {
                throw new UserFriendlyException("刷新token 过期");
            }

            tokenManager.ValidateAccessToken();

            var user = await context.User.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);

            if (user is null)
            {
                ThrowUserFriendlyException.ThrowException("用户不存在");
            }

            var clamis = new List<Claim>()
            {
                new Claim(type:UserInfomationConst.UserName,user!.UserName),
                new Claim(type:UserInfomationConst.UserAccount,user.UserAccount),
                new Claim(type:UserInfomationConst.UserId,user.Id),
            };

            return new RefreshTokenDto
            {
                RefresheToken = tokenManager.GenerateAccessToken(clamis)
            };
        }
    }
}
