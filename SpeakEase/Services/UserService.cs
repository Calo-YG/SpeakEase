using System.Text.RegularExpressions;
using FastService;
using IdGen;
using Lazy.Captcha.Core;
using Microsoft.EntityFrameworkCore;
using SpeakEase.Contract.Users;
using SpeakEase.Contract.Users.Dto;
using SpeakEase.Domain.Users;
using SpeakEase.Domain.Users.Enum;
using SpeakEase.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Filters;
using SpeakEase.Infrastructure.SpeakEase.Core;

namespace SpeakEase.Services
{
    /// <summary>
    /// 用户服务类
    /// </summary>
    [Filter(typeof(ResultEndPointFilter))]
    [Route("api/user")]
    [Tags("用户服务")]
    public class UserService(ICaptcha captcha,IDbContext dbContext,IdGenerator idgenerator): FastApi, IUserService
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [EndpointSummary("注册")]
        public async Task Register(CreateUserRequest request)
        {
            if (request.Email.IsNullOrEmpty() || !Regex.IsMatch(request.Email, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
            {
                ThrowUserFriendlyException.ThrowException("邮箱格式不正确");
            }

            if (request.Password.IsNullOrEmpty() 
                || request.Password.Length < 6 ||
               !Regex.IsMatch(request.Password, @"^(?=.*[0-9])(?=.*[a-zA-Z]).*$"))
            {
                ThrowUserFriendlyException.ThrowException("密码长度至少6位，且必须包含字母和数字");
            }

            if (request.UnquneId.IsNullOrEmpty() || request.VerificationCode.IsNullOrEmpty())
            {
                ThrowUserFriendlyException.ThrowException("请输入验证码");
            }

            if (request.UserAccount.IsNullOrEmpty() || request.UserName.IsNullOrEmpty())
            {
                ThrowUserFriendlyException.ThrowException("请输入账号和用户名");
            }

            var validate = captcha.Validate(request.UnquneId, request.VerificationCode);

            if (!validate)
            {
                ThrowUserFriendlyException.ThrowException("验证码校验错误");
            }

            var any = await dbContext.User.AsNoTracking().AnyAsync(p=>p.UserAccount == request.UserAccount || p.UserName == request.UserName);

            if (any)
            {
                ThrowUserFriendlyException.ThrowException("当前账号和用户名已存在，请重新输入");
            }

            var entity = new UserEntity(idgenerator.CreateId(),
                request.UserName,
                request.UserAccount,
                BCrypt.Net.BCrypt.HashPassword(request.Password),
                request.Email,
                string.Empty,string.
                Empty,
                SourceEnum.Register);

            await dbContext.User.AddAsync(entity);

            await dbContext.SaveChangesAsync();
        }
    }
}
