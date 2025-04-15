using System.Text.RegularExpressions;
using IdGen;
using Lazy.Captcha.Core;
using Microsoft.EntityFrameworkCore;
using SpeakEase.Contract.Users;
using SpeakEase.Contract.Users.Dto;
using SpeakEase.Domain.Users;
using SpeakEase.Domain.Users.Const;
using SpeakEase.Domain.Users.Enum;
using SpeakEase.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Redis;
using SpeakEase.Infrastructure.SpeakEase.Core;

namespace SpeakEase.Services
{
    /// <summary>
    /// 用户服务类
    /// </summary>
    public class UserService(ICaptcha captcha,IDbContext dbContext,IdGenerator idgenerator,IRedisService redisService,IWebHostEnvironment webHostEnvironment): IUserService
    {
        /// <summary>
        /// 头像类型限制
        /// </summary>
        private readonly string[] _fileType = ["png", "jpg", "jpeg"];

        /// <summary>
        /// 头像文件大小限制
        /// </summary>
        private readonly int fileSize = 3078;

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

            var key = string.Format(UserConst.RegiesCapchaCache, request.UnquneId);

            var code = redisService.Get(key);

            var validate = captcha.Validate(request.UnquneId, request.VerificationCode);

            if (code != request.VerificationCode)
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

            await redisService.DeleteAsync(key);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [EndpointSummary("修改用户密码")]
        public async Task ModifyPassword(UpdateUserRequest request)
        {
            if (request.Password.IsNullOrEmpty()
                   || request.Password.Length < 6 ||
                   !Regex.IsMatch(request.Password, @"^(?=.*[0-9])(?=.*[a-zA-Z]).*$"))
            {
                ThrowUserFriendlyException.ThrowException("密码长度至少6位，且必须包含字母和数字");
            }

            if(request.OldPassword.IsNullOrEmpty()
                   || request.OldPassword.Length < 6 ||
                   !Regex.IsMatch(request.OldPassword, @"^(?=.*[0-9])(?=.*[a-zA-Z]).*$"))
            {
                ThrowUserFriendlyException.ThrowException("旧密码密码长度至少6位，且必须包含字母和数字");
            }

            var currentuser = dbContext.GetUser();

            var user =await dbContext.User.FirstAsync(p => p.Id == currentuser.Id);

            var checkpassword = BCrypt.Net.BCrypt.Verify(request.OldPassword, user.UserPassword);

            if(!checkpassword)
            {
                ThrowUserFriendlyException.ThrowException("旧密码校验错误");
            }

            user.Modify(DateTime.Now, BCrypt.Net.BCrypt.HashPassword(request.Password));

            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [EndpointSummary("当前登录用户信息")]
        public async Task<UserResponse> CurrentUser()
        {
            var user = dbContext.GetUser();

            return await dbContext.QueryNoTracking<UserEntity>().Where(p=>p.Id == user.Id).Select(p=> new UserResponse
            {
                UserId = p.Id,
                UserName = p.UserName,
                Email = p.Email,
                Avatar =p.Avatar,
                Phone = p.Phone,
            }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [EndpointSummary("上传头像")]
        public async Task Uploadavatar(IFormFile file)
        {
            if (file is null)
            {
                ThrowUserFriendlyException.ThrowException("请选择头像上传");
            }

            var suffix = file.FileName.Split('.')[1];

            if (!_fileType.Contains(suffix))
            {
                ThrowUserFriendlyException.ThrowException("只支持 png,jpg,jpeg 文件类型上传");
            }

            var rootpath = "wwwroot/Avatar";

            var path = Path.Join(webHostEnvironment.ContentRootPath,rootpath);

            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var currentuser = dbContext.GetUser();

            var userentity = dbContext.User.First(p=>p.Id == currentuser.Id);

            var key = LongToStringConverter.Convert(idgenerator.CreateId());

            var filename = $"{currentuser.Name}_{key}.{suffix}";

            var filepath = Path.Join(path, $"{currentuser.Name}_{key}.{suffix}");

            using var filestream = new FileStream(filepath, FileMode.Create);

            await file.CopyToAsync(filestream);

            userentity.SetAvatar(Path.Join($"/{rootpath}/{filename}"));

            await dbContext.SaveChangesAsync(); 
        }

        /// <summary>
        /// 创建当前用户请求
        /// </summary>
        /// <returns></returns>
        [EndpointSummary("创建当前用户请求")]
        public async Task CreateUserSetting(UserSettingRequest request)
        {
            var id = idgenerator.CreateId();

            var user = dbContext.GetUser();

            var entity = new UserSettingEntity(id,user.Id,
                request.Gender ?? string.Empty,
                request.Birthday,
                request.Bio ?? string.Empty,
                request.IsProfilePublic,
                request.ShowLearningProgress,
                request.AllowMessages,
                request.ReceiveNotifications,
                request.ReceiveEmailUpdates,
                request.ReceivePushNotifications,
                request.AccountActive);

            await dbContext.UserSetting.AddAsync(entity);

            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 创建当前用户请求
        /// </summary>
        /// <returns></returns>
        [EndpointSummary("更新当前用户请求")]
        public async Task UpdateUserSetting(UserSettingUpdateRequest request)
        {
            var entity = await dbContext.UserSetting.FirstAsync(p=>p.Id == request.Id);

            if(entity == null)
            {
                ThrowUserFriendlyException.ThrowException("数据错误");
            }

            entity.Modify(request.Gender ?? string.Empty,
                request.Birthday,
                request.Bio ?? string.Empty,
                request.IsProfilePublic,
                request.ShowLearningProgress,
                request.AllowMessages,
                request.ReceiveNotifications,
                request.ReceiveEmailUpdates,
                request.ReceivePushNotifications,
                request.AccountActive);

            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        [EndpointSummary("获取当前用户设置")]
        public  Task<UserSettingResponse> GetUserSetting()
        {
            //获取当前用户
            var user = dbContext.GetUser();

            return dbContext.UserSetting.Where(p=>p.UserId == user.Id)
                .Select(p=> new UserSettingResponse
                {
                    Gender = p.Gender,
                    Birthday = p.Birthday,
                    Bio = p.Bio,
                    IsProfilePublic = p.IsProfilePublic,
                    ShowLearningProgress = p.ShowLearningProgress,
                    AllowMessages = p.AllowMessages,
                    ReceiveEmailUpdates = p.ReceiveEmailUpdates,
                    ReceiveNotifications = p.ReceiveNotifications,
                    ReceivePushNotifications = p.ReceivePushNotifications,
                    AccountActive = p.AccountActive
                }).FirstAsync();
        }
    }
}
