using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SpeakEase.Authorization.Authorization;
using SpeakEase.Gateway.Application.App;
using SpeakEase.Gateway.Application.Cluster;
using SpeakEase.Gateway.Application.Route;
using SpeakEase.Gateway.Application.SysUser;
using SpeakEase.Gateway.Contract.App;
using SpeakEase.Gateway.Contract.Cluster;
using SpeakEase.Gateway.Contract.Route;
using SpeakEase.Gateway.Contract.SysUser;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Gateway.MapRoute;
using SpeakEase.Infrastructure.Middleware;
using SpeakEase.Infrastructure.Options;
using SpeakEase.Infrastructure.Redis;
using SpeakEase.Infrastructure.WorkIdGenerate;
using SpeakEase.Gateway.Infrastructure.MassTransit.Message;

string cors ="SpeakEase.Gateway";

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
#region 配置 Json Web Token (JWT)

var options = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

var secret = options!.SecretKey;
var issuer = options.Issuer;
var audience = options.Audience;
var expire = options.ExpMinutes;


if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new Exception("validate jwt options failed");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(op =>
    {
        op.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true, // 检查过期时间
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            ClockSkew = TimeSpan.FromSeconds(expire),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });

builder.Services.RegisterAuthorization();
builder.Services.AddAuthorization();

#endregion

#region 跨域配置

builder.Services.AddCors(opt =>opt.AddPolicy(cors,policy =>
    policy
        .WithOrigins("http://localhost:8080","https://app.apifox.com") // 允许所有来源
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials())
);


#endregion

#region 配置masstransit
builder.Services.BuildMassTransitWithRabbitMq();
#endregion

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseCors(cors);

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(op =>
    {
        op.WithTitle("SpeakEase.Gateway");
        op.WithTheme(ScalarTheme.Moon);
    }); 
    app.MapOpenApi();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("SpeakEase/health", () => Results.Ok("SpeakEase.Gateway"));
app.MapAppEndPoint();
app.MapRouteEndPoint();
app.MapClusterEndPoint();
app.MapSysUserEndPoint();

await app.RunAsync();
