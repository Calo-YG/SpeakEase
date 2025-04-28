using System.Text;
using System.Text.Json;
using IdGen;
using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using SpeakEase.Contract.Auth;
using SpeakEase.Contract.Users;
using SpeakEase.Infrastructure.Authorization;
using SpeakEase.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.EventBus;
using SpeakEase.Infrastructure.Files;
using SpeakEase.Infrastructure.Filters;
using SpeakEase.Infrastructure.Middleware;
using SpeakEase.Infrastructure.Redis;
using SpeakEase.MapRoute;
using SpeakEase.Services;

namespace SpeakEase;

internal class Program
{
    public static async Task Main(string[] args)
    {
        #region configure serilog

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/LogInformation.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
            .WriteTo.File("Logs/LogError.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.File("Logs/LogWarning.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Warning)
            .CreateLogger();

        #endregion

        try
        {
            Log.Information("Starting web host");

            var builder = WebApplication.CreateSlimBuilder(args);

            builder.Host.UseSerilog(
                (context, services, configuration) =>
                    configuration.ReadFrom
                        .Configuration(context.Configuration)
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .ReadFrom.Services(services)
                        .Enrich.FromLogContext()
                        //.WriteTo.File("Logs/LogInformation.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
                        .WriteTo.File("Logs/LogError.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error)
                        .WriteTo.File("Logs/LogWarning.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Warning)
                        .WriteTo.Console());

            builder.AddServiceDefaults();

            builder.Services.AddRouting();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddScoped<ExceptionMiddleware>();

            var cors = "SpeakEase";

            var configuration = builder.Configuration;

            builder.Services.AddHttpContextAccessor();
            builder.Services.RegisterEntityFrameworkCoreContext(builder.Configuration);

            #region configure json

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.Converters.Add(new DateTimeOffsetConverter());
                options.SerializerOptions.Converters.Add(new DateTimeOffsetNullableConverter());
            });

            #endregion

            #region configure cors

            builder.Services.AddCors(
                options =>
                    options.AddPolicy(
                        cors,
                        builder =>
                            builder
                                .WithOrigins(
                                    ["https://localhost:3000", "http://localhost:3000", "http://localhost:8080"]
                                )
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                    )
            );

            #endregion

            #region configure authentication--jwt

            var secret = configuration["App:JwtOptions:SecretKey"];
            var issuser = configuration["App:JwtOptions:Issuer"];
            var audience = configuration["App:JwtOptions:Audience"];

            var expire = configuration.GetSection("App:JwtOptions:ExpMinutes").Get<int?>() ?? 30;

            builder.Services.Configure<JwtOptions>(configuration.GetSection("App:JwtOptions"));

            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuser) || string.IsNullOrEmpty(audience))
            {
                throw new Exception("validate jwt options failed");
            }

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true, // 检查过期时间
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuser,
                        ValidAudience = audience,
                        ClockSkew = TimeSpan.FromSeconds(expire),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });

            builder.Services.RegisterAuthorizetion();
            builder.Services.AddAuthorization();
            #endregion

            #region configure localeventbus

            builder.Services.RegisterLocalEventBus();

            #endregion

            #region DistributedCache
            //builder.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = builder.Configuration.GetConnectionString("App:RedisConn");
            //    options.InstanceName = "SpeakEase";
            //});
            #endregion

            #region VerificationCode
            builder.Services.AddCaptcha(builder.Configuration);
            #endregion

            #region SnowflakeId


            builder.Services.AddIdGen(123, () => new IdGeneratorOptions()); // Where 123 is the generator-id

            #endregion

            #region scalar

            builder.Services.AddOpenApi(options =>
            {

            });
            #endregion

            #region redis
            builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));

            builder.Services.AddSingleton<IRedisService, RedisService>();
            #endregion

            #region build service
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            #endregion 

            #region 文件存储
            builder.Services.Configure<FileOption>(builder.Configuration.GetSection("FileOption"));
            // builder.Services.AddTransient<IFileProvider, DefaultFileProvider>();
            #endregion

            builder.Services.AddHttpLogging(options =>
            {
                options.CombineLogs = false;
                options.RequestHeaders.Clear();
                options.ResponseHeaders.Clear();
            });

            var app = builder.Build();

            app.UseHttpLogging();

            app.MapDefaultEndpoints();

            app.UseCors(cors);

            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference(options =>
                {
                    options.AddHttpAuthentication("Bearer ", cfg =>
                    {

                    });
                    options.WithTitle("SpeakEase");
                    options.WithTheme(ScalarTheme.Moon);
                    options.WithModels(true);
                }); // scalar/v1
                app.MapOpenApi();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
                RequestPath = "/wwwroot"
            });

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapGet("SpeakEase/health", (IDbContext context) => Results.Ok("SpeakEase"));


            app.MapAuthEndponit();
            app.MapUserEndpoint();
            
            await app.RunAsync();

            Log.Information("Started web host");
        }
        catch (Exception e)
        {
            Log.Error("An error occurred while starting the web host", e.Message);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}