using SpeakEase.Gateway.Contract.SysUser;
using SpeakEase.Gateway.Contract.SysUser.Dto;
using SpeakEase.Gateway.Domain.Entity.System;
using SpeakEase.Gateway.Filter;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
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

        group.MapPost("login", (ISysUserService sysUserService, LoginInput input) => sysUserService.LoginAsync(input)).WithSummary("登录")
            .RequireOperateLog("sysuser", "Login");
        
        group.MapPost("register", (ISysUserService sysUserService, CreateUserInput input) => sysUserService.CreateUserAsync(input)).WithSummary("注册")
            .RequireOperateLog("sysuser", "Register");
        
        group.MapPost("update", (ISysUserService sysUserService, UpdateUserInput input) => sysUserService.UpdateUserAsync(input)).WithSummary("更新用户信息")
            .RequireOperateLog("sysuser", "Update");
        
        group.MapDelete("delete/{id}", (ISysUserService sysUserService, string id) => sysUserService.DeleteUserAsync(id)).WithSummary("删除").RequireAuthorization()
            .RequireOperateLog("sysuser", "Delete");
        
        group.MapGet("getDetail", (ISysUserService sysUserService) => sysUserService.GetUserDetailAsync()).WithSummary("获取详情").RequireAuthorization()
            .RequireOperateLog("sysuser", "GetDetail");
        
        group.MapPost("getList", (ISysUserService sysUserService, UserPageInput input) => sysUserService.GetListAsync(input)).WithSummary("分页列表")
            .RequireAuthorization().RequireOperateLog("sysuser", "GetList");
        
        group.MapPost("logout", (ISysUserService sysUserService) => sysUserService.LogOutAsync()).WithSummary("登出").RequireAuthorization()
            .RequireOperateLog("sysuser", "LogOut");
        
        group.MapPost("refreshToken", (ISysUserService sysUserService, RefreshTokenInput input) => sysUserService.RefreshTokenAsync(input))
            .WithSummary("刷新Token").RequireOperateLog("sysuser", "RefreshToken");
        
        group.MapPost("create", (ISysUserService sysUserService, CreateUserInput input) => sysUserService.CreateUserAsync(input)).WithSummary("添加用户")
            .RequireAuthorization().RequireOperateLog("sysuser", "Create");

        group.MapGet("userCount", (IDbContext dbContext) => dbContext.QueryNoTracking<SysUserEntity>().Count()).WithSummary("获取用户总数")
            .RequireAuthorization().RequireOperateLog("sysuser", "Create");
    }
}