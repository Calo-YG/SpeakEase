using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Gateway.Infrastructure.MassTransit;
using SpeakEase.Gateway.Infrastructure.Yarp.Core;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Host.UseAgileConfig(("Configurations/agileconfig.json"));

builder.Services.RegisterEntityFrameworkCoreContext(builder.Configuration);

#region 构建yarp 反向代理
builder.Services.AddReverseProxyWithDatabase();
#endregion

#region 配置masstransit
builder.Services.AddMassTransitWithRabbitMq();
#endregion

var app = builder.Build();

app.MapReverseProxy();

await app.RunAsync();
