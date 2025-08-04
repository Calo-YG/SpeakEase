using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Host.UseAgileConfig(("Configurations/agileconfig.json"));

builder.Services.RegisterEntityFrameworkCoreContext(builder.Configuration);

var app = builder.Build();

await app.RunAsync();
