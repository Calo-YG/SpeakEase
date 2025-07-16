using SpeakEase.Domain.Contract.Entity;

namespace SpeakEase.Gateway.Domain.Entity.Gateway;

/// <summary>
/// 集群基类
/// </summary>
public class ClusterEntity:SpeakEaseEntity
{
    /// <summary>
    /// 集群id
    /// </summary>
    public string ClusterId { get;private set; }

    /// <summary>
    /// 负载均衡策略
    /// </summary>
    public string LoadBalance { get;private set; }

    //
    // 摘要:
    //     是否启用健康检查
    public bool Enabled { get; private  set; }

    //
    // 摘要:
    //     健康检查间隔
    public int? Interval { get; private set; }

    //
    // 摘要:
    //     健康检查超时时间
    public int? Timeout { get; private set; }

    //
    // 摘要:
    //     健康检查策略
    public string Policy { get; private set; }

    //
    // 摘要:
    //     健康检查路径
    public string Path { get; private set; }

    /// <summary>
    /// 可用目的地策略
    /// </summary>
    public string AvailableDestinationsPolicy { get; private set; }
    
    /// <summary>
    /// 无参构造函数
    /// </summary>
    protected ClusterEntity()
    { 
    }
    
    /// <summary>
    /// 有参构造函数
    /// </summary>
    /// <param name="clusterId"></param>
    /// <param name="loadBalance"></param>
    /// <param name="enabled"></param>
    /// <param name="interval"></param>
    /// <param name="timeout"></param>
    /// <param name="policy"></param>
    /// <param name="path"></param>
    /// <param name="availableDestinationsPolicy"></param>
    public ClusterEntity(string clusterId, string loadBalance, bool enabled, int? interval, int? timeout, string policy, string path, string availableDestinationsPolicy)
    {
        ClusterId = clusterId;
        LoadBalance = loadBalance;
        Enabled = enabled;
        Interval = interval;
        Timeout = timeout;
        Policy = policy;
        Path = path;
        AvailableDestinationsPolicy = availableDestinationsPolicy;
    }
}