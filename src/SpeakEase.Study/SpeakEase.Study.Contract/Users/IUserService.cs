using Microsoft.AspNetCore.Http;
using SpeakEase.Study.Contract.Users.Dto;

namespace SpeakEase.Study.Contract.Users;

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
    Task Register(CreateUserInput request);

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>

    Task ModifyPassword(UpdateUserInput request);

    /// <summary>
    /// 当前登录用户信息
    /// </summary>
    /// <returns></returns>
    Task<UserDto> CurrentUser();

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
    Task CreateUserSetting(UserSettingInput request);

    /// <summary>
    /// 更新当前用户请求设置
    /// </summary>
    /// <returns></returns>
    Task UpdateUserSetting(UserSettingUpdateInput request);

    /// <summary>
    /// 获取当前用户
    /// </summary>
    /// <returns></returns>
    Task<UserSettingDto> GetUserSetting();

}