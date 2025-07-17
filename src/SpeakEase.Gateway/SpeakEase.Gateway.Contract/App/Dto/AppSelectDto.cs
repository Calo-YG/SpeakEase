namespace SpeakEase.Gateway.Contract.App.Dto;

/// <summary>
/// 应用选择
/// </summary>
public sealed class AppSelectDto
{
    /// <summary>
    /// 应用ID
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }
    
    /// <summary>
    /// 应用编码
    /// </summary>
    public string AppCode { get; set; }
}