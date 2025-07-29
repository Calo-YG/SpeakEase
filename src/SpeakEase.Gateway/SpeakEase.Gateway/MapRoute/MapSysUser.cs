using SpeakEase.Gateway.Contract.SysUser;
using SpeakEase.Gateway.Contract.SysUser.Dto;
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

        group.MapPost("login", (ISysUserService sysUserService, LoginInput input) => sysUserService.LoginAsync(input)).WithSummary("登录");
        group.MapPost("register", (ISysUserService sysUserService, CreateUserInput input) => sysUserService.CreateUserAsync(input)).WithSummary("注册");
        group.MapPost("update", (ISysUserService sysUserService, UpdateUserInput input) => sysUserService.UpdateUserAsync(input)).WithSummary("更新用户信息");
        group.MapDelete("delete", (ISysUserService sysUserService, string id) => sysUserService.DeleteUserAsync(id)).WithSummary("删除");
        group.MapGet("getDetail", (ISysUserService sysUserService) => sysUserService.GetUserDetailAsync()).WithSummary("获取详情");

    }
}