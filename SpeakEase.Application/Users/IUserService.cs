using SpeakEase.Contract.Users.Dto;

namespace SpeakEase.Contract.Users;

/// <summary>
/// 用户接口
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 用户注册请求
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task Create(CreateUserRequest request);
}