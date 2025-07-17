using SpeakEase.Gateway.Contract.Route;
using SpeakEase.Gateway.Contract.Route.Dto;
using SpeakEase.Gateway.Filter;

namespace SpeakEase.Gateway.MapRoute;

/// <summary>
/// 路由映射
/// </summary>
public static class MapRoute
{
    /// <summary>
    /// 路由映射
    /// </summary>
    /// <param name="app"></param>
    public static void MapRouteEndPoint(this IEndpointRouteBuilder app)
    { 
        var group = app.MapGroup("api/route")
            .WithDescription("路由管理")
            .WithTags("route")
            .AddEndpointFilter<ResultEndPointFilter>();

        group.MapPost("create", (IRouteService routeService, CreateRouteInput input) => routeService.CreateRouteAsync(input)).WithSummary("创建路由");
        group.MapPost("getList", (IRouteService routeService, RoutePageInput input) => routeService.GetListAsync(input)).WithSummary("分页列表");
        group.MapGet("getById/{id}", (IRouteService routeService, string id) => routeService.GetByIdAsync(id)).WithSummary("获取路由");
        group.MapPut("update", (IRouteService routeService, UpdateRouteInput input) => routeService.UpdateRouteAsync(input)).WithSummary("更新路由");
        group.MapDelete("delete/{id}", (IRouteService routeService, string id) => routeService.DeleteAsync(id)).WithSummary("删除路由");
    }
}