using FastService;
using SpeakEase.Contract.Users;
using SpeakEase.Contract.Users.Dto;

namespace SpeakEase.Services
{
    /// <summary>
    /// 用户服务类
    /// </summary>
    public class UserService: FastApi, IUserService
    {
        public Task Create(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
