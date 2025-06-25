using Scalar.AspNetCore;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAgileConfig(("Configurations/agileconfig.json"));

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services
    .AddHttpContextAccessor()
    .RegisterEntityFrameworkCoreContext(builder.Configuration);

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

await app.RunAsync();
