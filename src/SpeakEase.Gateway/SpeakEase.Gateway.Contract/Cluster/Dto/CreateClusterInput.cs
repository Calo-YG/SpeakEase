namespace SpeakEase.Gateway.Contract.Cluster.Dto;

/// <summary>
/// 集群创建
/// </summary>
public sealed class CreateClusterInput
{
    /// <summary>
    /// 集群id
    /// </summary>
    public string ClusterId { get; set; }
    
    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 负载均衡策略
    /// </summary>
    public string LoadBalance { get; set; }

    //
    // 摘要:
    //     是否启用健康检查
    public bool Enabled { get; set; }

    //
    // 摘要:
    //     健康检查间隔
    public int? Interval { get; set; }

    //
    // 摘要:
    //     健康检查超时时间
    public int? Timeout { get; set; }

    //
    // 摘要:
    //     健康检查策略
    public string Policy { get; set; }

    //
    // 摘要:
    //     健康检查路径
    public string Path { get; set; }

    /// <summary>
    /// 可用目的地策略
    /// </summary>
    public string AvailableDestinationsPolicy { get; set; }
}