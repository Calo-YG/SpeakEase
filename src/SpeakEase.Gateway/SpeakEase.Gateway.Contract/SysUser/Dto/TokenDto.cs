namespace SpeakEase.Gateway.Contract.SysUser.Dto;

/// <summary>
/// 登录返回
/// </summary>
public sealed class TokenDto
{
    /// <summary>
    /// 令牌
    /// </summary>
    public string Token { get; set; }
    
    /// <summary>
    /// 刷新令牌
    /// </summary>
    public string RefreshToken { get; set; }
}