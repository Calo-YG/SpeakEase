namespace SpeakEase.Gateway.Contract.SysUser.Dto;

/// <summary>
/// 创建用户输入
/// </summary>
public sealed class CreateUserInput
{
    /// <summary>
    /// 用户主键
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; }
    
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