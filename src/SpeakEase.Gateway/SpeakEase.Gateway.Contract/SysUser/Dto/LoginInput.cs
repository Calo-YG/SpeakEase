namespace SpeakEase.Gateway.Contract.SysUser.Dto;

/// <summary>
/// 登录
/// </summary>
public sealed class LoginInput
{
    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; }
    
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}