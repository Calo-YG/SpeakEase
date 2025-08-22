using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Gateway.Contract.SysUser.Dto;

/// <summary>
/// 用户分页查询参数
/// </summary>
public sealed class UserPageInput
{
    /// <summary>
    /// 关键字
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }
    
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
    
    /// <summary>
    /// 分页
    /// </summary>
    public Pagination Pagination { get; set; }
}