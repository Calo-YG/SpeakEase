using Scalar.AspNetCore;
using SpeakEase.Gateway.Application.App;
using SpeakEase.Gateway.Application.Cluster;
using SpeakEase.Gateway.Application.Route;
using SpeakEase.Gateway.Application.SysUser;
using SpeakEase.Gateway.Contract.App;
using SpeakEase.Gateway.Contract.Cluster;
using SpeakEase.Gateway.Contract.Route;
using SpeakEase.Gateway.Contract.SysUser;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Gateway.Infrastructure.Yarp.Core;
using SpeakEase.Gateway.MapRoute;
using SpeakEase.Infrastructure.Middleware;
using SpeakEase.Infrastructure.Redis;
using SpeakEase.Infrastructure.WorkIdGenerate;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAgileConfig(("Configurations/agileconfig.json"));

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services
    .AddHttpContextAccessor()
    .RegisterEntityFrameworkCoreContext(builder.Configuration);
builder.Services.AddScoped<ExceptionMiddleware>();
#region 配置redis
builder.Services.AddRedis(builder.Configuration);

#endregion
#region 配置雪花id

builder.Services.AddIdGenerate(builder.Configuration);

#endregion
#region 手动注册应用服务
builder.Services.AddScoped<IAppService, AppService>();
builder.Services.AddScoped<IClusterService, ClusterService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<ISysUserService, SysUserService>();
#endregion
#region 配置yarp 反向代理
builder.Services.AddReverseProxyWithDatabase();
#endregion

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("SpeakEase.Gateway");
        options.WithTheme(ScalarTheme.Moon);
    }); 
    app.MapOpenApi();
}

app.MapGet("SpeakEase/health", () => Results.Ok("SpeakEase.Gateway"));
app.MapAppEndPoint();
app.MapRouteEndPoint();
app.MapClusterEndPoint();
app.MapSysUserEndPoint();

await app.RunAsync();
