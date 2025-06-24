using System.ComponentModel.DataAnnotations.Schema;
using SpeakEase.Domain.Contract.Entity;

namespace SpeakEase.Gateway.Domain.Entity.Gateway;

/// <summary>
/// 路由实体
/// </summary>
[Table("route")]
public class RouterEntity:SpeakEaseEntity
{
    /// <summary>
    /// 应用id
    /// </summary>
    public string AppId { get; set; }
    
    /// <summary>
    /// 路由前缀
    /// </summary>
    public string Prefix { get; set; }
    
    /// <summary>
    /// 集群配置
    /// </summary>
    public string ClusterId { get; set; }
    
    /// <summary>
    /// 匹配路径
    /// </summary>
    public string Path { get; set; }
    
    protected RouterEntity()
    {
        
    }
    /// <summary>
    /// 创建路由
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="prefix"></param>
    /// <param name="clusterId"></param>
    /// <param name="path"></param>
    public RouterEntity(string appId, string prefix, string clusterId, string path)
    {
        AppId = appId;
        Prefix = prefix;
        ClusterId = clusterId;
        Path = path;
    }
}