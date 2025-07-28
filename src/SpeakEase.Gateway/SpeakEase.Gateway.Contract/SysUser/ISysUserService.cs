using SpeakEase.Gateway.Contract.SysUser.Dto;
using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Gateway.Contract.SysUser;

public interface ISysUserService
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task CreateUserAsync(CreateUserInput input);
    
    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateUserAsync(UpdateUserInput input);
    
    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteUserAsync(string id);
    
    /// <summary>
    /// 获取用户详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserDetailDto> GetUserDetailAsync(string id);
    
    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<TokenDto> LoginAsync(LoginInput input);
    
    /// <summary>
    /// 用户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageResult<UserPageDto>> GetListAsync(UserPageInput input);
}