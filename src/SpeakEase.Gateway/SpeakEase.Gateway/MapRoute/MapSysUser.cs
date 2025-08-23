using SpeakEase.Gateway.Contract.SysUser;
using SpeakEase.Gateway.Contract.SysUser.Dto;
using SpeakEase.Gateway.Filter;
using SpeakEase.Gateway.Infrastructure.GatewayLog;

namespace SpeakEase.Gateway.MapRoute;

public static class MapSysUser
{
    public static void MapSysUserEndPoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/sysuser")
            .WithDescription("系统用户管理")
            .WithTags("sysuser")
            .AddEndpointFilter<OperateLogEndpointFilter>()
            .AddEndpointFilter<ResultEndPointFilter>();

        group.MapPost("login", (ISysUserService sysUserService, LoginInput input) => sysUserService.LoginAsync(input)).WithSummary("登录").RequireOperateLog("Auth","Login");
        group.MapPost("register", (ISysUserService sysUserService, CreateUserInput input) => sysUserService.CreateUserAsync(input)).WithSummary("注册").RequireOperateLog("Auth", "Register");
        group.MapPost("update", (ISysUserService sysUserService, UpdateUserInput input) => sysUserService.UpdateUserAsync(input)).WithSummary("更新用户信息").RequireOperateLog("Auth", "Update");
        group.MapDelete("delete", (ISysUserService sysUserService, string id) => sysUserService.DeleteUserAsync(id)).WithSummary("删除").RequireAuthorization().RequireOperateLog("Auth", "Delete");
        group.MapGet("getDetail", (ISysUserService sysUserService) => sysUserService.GetUserDetailAsync()).WithSummary("获取详情").RequireAuthorization().RequireOperateLog("Auth", "GetDetail");
        group.MapPost("getList", (ISysUserService sysUserService, UserPageInput input) => sysUserService.GetListAsync(input)).WithSummary("分页列表").RequireAuthorization().RequireOperateLog("Auth", "GetList");
        group.MapPost("logout", (ISysUserService sysUserService) => sysUserService.LogOutAsync()).WithSummary("登出").RequireAuthorization().RequireOperateLog("Auth", "LogOut");
        group.MapPost("refreshToken", (ISysUserService sysUserService, RefreshTokenInput input) => sysUserService.RefreshTokenAsync(input)).WithSummary("刷新Token").RequireOperateLog("Auth", "RefreshToken");

    }
}