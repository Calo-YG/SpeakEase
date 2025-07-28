namespace SpeakEase.Gateway.Contract.SysUser.Dto;

/// <summary>
/// 更新用户创建
/// </summary>
public sealed class UpdateUserInput
{
    /// <summary>
    /// 主键
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// 用户名称
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 用户头像
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// 用户头像
    /// </summary>
    public string Avatar { get; set; }
}