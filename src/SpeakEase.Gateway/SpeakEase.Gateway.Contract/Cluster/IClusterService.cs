using SpeakEase.Gateway.Contract.Cluster.Dto;

namespace SpeakEase.Gateway.Contract.Cluster;

/// <summary>
/// 集群服务
/// </summary>
public interface IClusterService
{
    /// <summary>
    /// 创建集群
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task CreateClusterAsync(CreateClusterInput input);

    /// <summary>
    /// 删除集群
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteClusterAsync(string id);

    /// <summary>
    /// 更新集群
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateClusterAsync(UpdateClusterInput input);
    
    /// <summary>
    /// 获取集群信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ClusterDto> GetByIdAsync(string id);
}