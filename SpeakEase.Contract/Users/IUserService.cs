using Microsoft.AspNetCore.Http;
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
    /// <returns></returns>
    Task<UserResponse> CurrentUser();

    /// <summary>
    /// 上传用户头像
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task Uploadavatar(IFormFile file);


    /// <summary>
    /// 创建当前用户设置请求
    /// </summary>
    /// <returns></returns>
    Task CreateUserSetting(UserSettingRequest request);

    /// <summary>
    /// 更新当前用户请求设置
    /// </summary>
    /// <returns></returns>
    Task UpdateUserSetting(UserSettingUpdateRequest request);

    /// <summary>
    /// 获取当前用户
    /// </summary>
    /// <returns></returns>
    Task<UserSettingResponse> GetUserSetting();

}