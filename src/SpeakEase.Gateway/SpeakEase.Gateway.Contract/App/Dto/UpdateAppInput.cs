namespace SpeakEase.Gateway.Contract.App.Dto;

public sealed class UpdateAppInput
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
}