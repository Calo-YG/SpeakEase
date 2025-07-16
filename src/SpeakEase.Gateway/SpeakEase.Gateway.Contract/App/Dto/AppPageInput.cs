using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Gateway.Contract.App.Dto;

public sealed class AppPageInput
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }
    
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
    
    /// <summary>
    /// 关键字
    /// </summary>
    public string Keyword { get; set; }
    
    /// <summary>
    /// 分页参数
    /// </summary>
    public Pagination Pagination { get; set; }
}