namespace SpeakEase.Infrastructure.WorkIdGenerate;

/// <summary>
/// 工作Id生成配置    
/// </summary>
public class WorkIdGenerateOptions
{
    /// <summary>
    /// id
    /// </summary>
    public string AppName { get; set; }
    
    /// <summary>
    /// redis key 前缀    
    /// </summary>
    public string RedisKeyPrefix { get; set; }
    
    /// <summary>
    /// 工作Id位长
    /// </summary>
    public byte WorkerIdBitLength { get; set; }
    
    /// <summary>
    /// 最小序列号
    /// </summary>
    public ushort MinSeqNumber { get; set; }
    
    /// <summary>
    /// 最大序列号
    /// </summary>
    public int MaxSeqNumber { get; set; }
    
    /// <summary>
    /// workId会话刷新间隔秒数
    /// </summary>
    public int SessionRefreshIntervalSeconds { get; set; }
}