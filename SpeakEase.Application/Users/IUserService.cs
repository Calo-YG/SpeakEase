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
    Task Register(CreateUserRequest request);

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>

    Task ModifyPassword(UpdateUserRequest request);

    /// <summary>
    /// 当前登录用户信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<UserResponse> CurrentUser(long id);

}