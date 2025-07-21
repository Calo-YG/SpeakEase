namespace SpeakEase.Infrastructure.WorkIdGenerate;

/// <summary>
/// 工作Id生成  
/// </summary>
public interface IWorkIdGenerate
{
    /// <summary>
    /// 获取workid
    /// </summary>
    /// <returns></returns>
    public ushort GetWorkId();
    
    /// <summary>
    /// workid配置
    /// </summary>
    public WorkIdGenerateOptions Options { get; }   
}