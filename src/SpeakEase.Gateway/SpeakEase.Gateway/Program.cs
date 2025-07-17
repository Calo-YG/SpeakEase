using Scalar.AspNetCore;
using SpeakEase.Gateway.Application.App;
using SpeakEase.Gateway.Application.Cluster;
using SpeakEase.Gateway.Application.Route;
using SpeakEase.Gateway.Contract.App;
using SpeakEase.Gateway.Contract.Cluster;
using SpeakEase.Gateway.Contract.Route;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Gateway.MapRoute;
using Yitter.IdGenerator;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAgileConfig(("Configurations/agileconfig.json"));

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services
    .AddHttpContextAccessor()
    .RegisterEntityFrameworkCoreContext(builder.Configuration);

#region 配置雪花id

var idGeneratorOptions = new IdGeneratorOptions(61) { WorkerIdBitLength = 6 };
YitIdHelper.SetIdGenerator(idGeneratorOptions);

#endregion

#region 手动注册应用服务

builder.Services.AddScoped<IAppService, AppService>();
builder.Services.AddScoped<IClusterService, ClusterService>();
builder.Services.AddScoped<IRouteService, RouteService>();

#endregion

var app = builder.Build();

app.MapDefaultEndpoints();

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

await app.RunAsync();
