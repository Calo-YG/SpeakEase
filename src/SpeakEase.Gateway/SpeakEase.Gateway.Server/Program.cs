using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Gateway.Infrastructure.Yarp.Core;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Host.UseAgileConfig(("Configurations/agileconfig.json"));

builder.Services.RegisterEntityFrameworkCoreContext(builder.Configuration);

#region 配置yarp 反向代理
builder.Services.AddReverseProxyWithDatabase();
#endregion

var app = builder.Build();

app.MapReverseProxy();

await app.RunAsync();
