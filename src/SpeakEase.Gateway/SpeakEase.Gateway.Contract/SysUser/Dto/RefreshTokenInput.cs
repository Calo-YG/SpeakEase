namespace SpeakEase.Gateway.Contract.SysUser.Dto;

/// <summary>
/// 刷新令牌
/// </summary>
public sealed class RefreshTokenInput
{
    /// <summary>
    /// 用户id
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// 刷新令牌
    /// </summary>
    public string RefreshToken { get; set; }
}