using System.Runtime.InteropServices.JavaScript;

namespace SpeakEase.Gateway.Contract.App.Dto;

/// <summary>
/// 应用分页
/// </summary>
public sealed class AppPageDto
{
    /// <summary>
    /// 主键id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }
    
    /// <summary>
    /// 应用密钥
    /// </summary>
    public string AppKey { get; set; }
    
    /// <summary>
    /// 应用编码
    /// </summary>
    public string AppCode { get; set; }
    
    /// <summary>
    /// 应用描述
    /// </summary>
    public string AppDescription { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}