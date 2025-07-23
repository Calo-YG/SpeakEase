using SpeakEase.Gateway.Filter;

namespace SpeakEase.Gateway.MapRoute;

public static class MapSysUser
{
    public static void MapSysUserEndPoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/sysuser")
            .WithDescription("系统用户管理")
            .WithTags("sysuser")
            .AddEndpointFilter<ResultEndPointFilter>();
        
        
    }
}