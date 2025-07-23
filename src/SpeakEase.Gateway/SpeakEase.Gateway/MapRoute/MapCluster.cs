using SpeakEase.Gateway.Contract.Cluster;
using SpeakEase.Gateway.Contract.Cluster.Dto;
using SpeakEase.Gateway.Filter;

namespace SpeakEase.Gateway.MapRoute;

public static class MapCluster
{
    public static void MapClusterEndPoint(this IEndpointRouteBuilder app)
    { 
        var group = app.MapGroup("api/cluster")
            .WithDescription("集群管理")
            .WithTags("cluster")
            .AddEndpointFilter<ResultEndPointFilter>();

        group.MapPost("create", (IClusterService clusterService, CreateClusterInput input) => clusterService.CreateClusterAsync(input)).WithSummary("创建集群");
        group.MapPost("update", (IClusterService clusterService, UpdateClusterInput input) => clusterService.UpdateClusterAsync(input)).WithSummary("更新集群");
        group.MapPost("delete", (IClusterService clusterService, string id) => clusterService.DeleteClusterAsync(id)).WithSummary("删除集群");
        group.MapGet("getById", (IClusterService clusterService, string id) => clusterService.GetByIdAsync(id)).WithSummary("获取集群");
    }
}

