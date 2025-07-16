using SpeakEase.Gateway.Contract.App;
using SpeakEase.Gateway.Contract.App.Dto;
using SpeakEase.Gateway.Filter;

namespace SpeakEase.Gateway.MapRoute;

public static class MapApp
{
    /// <summary>
    /// 映射 App
    /// </summary>
    /// <param name="app"></param>
    public static void MapAppEndPoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/app")
            .WithDescription("应用管理")
            .WithTags("app")
            .AddEndpointFilter<ResultEndPointFilter>();
        
        group.MapPost("create", (IAppService appService, CreateAppInput input) => appService.CreateAppAsync(input)).WithSummary("创建应用");
        group.MapPost("getList", (IAppService appService, AppPageInput input) => appService.GetList(input)).WithSummary("分页列表");
    }
}