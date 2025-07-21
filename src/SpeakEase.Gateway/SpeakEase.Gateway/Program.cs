using Scalar.AspNetCore;
using SpeakEase.Gateway.Application.App;
using SpeakEase.Gateway.Application.Cluster;
using SpeakEase.Gateway.Application.Route;
using SpeakEase.Gateway.Contract.App;
using SpeakEase.Gateway.Contract.Cluster;
using SpeakEase.Gateway.Contract.Route;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Gateway.MapRoute;
using SpeakEase.Infrastructure.Middleware;
using SpeakEase.Infrastructure.Options;
using SpeakEase.Infrastructure.Redis;
using SpeakEase.Infrastructure.WorkIdGenerate;
using Yitter.IdGenerator;

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
builder.Services.AddScoped<IRouteService, RouteService>();
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


await app.RunAsync();
