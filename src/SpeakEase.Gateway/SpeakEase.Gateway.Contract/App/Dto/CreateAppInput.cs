namespace SpeakEase.Gateway.Contract.App.Dto;

/// <summary>
/// 创建应用输入
/// </summary>
public sealed class CreateAppInput
{
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
}