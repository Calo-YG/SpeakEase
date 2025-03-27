using System.Reflection;
using System.Text;
using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using IdGen;
using IdGen.DependencyInjection;
using Lazy.Captcha.Core.Generator;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using SpeakEase.Infrastructure.Authorization;
using SpeakEase.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.EventBus;
using SpeakEase.Infrastructure.Middleware;
using SpeakEase.Infrastructure.Shared;
using SpeakEase.Infrastructure.SpeakEase.Core;
using Swashbuckle.AspNetCore.Filters;

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
                        .WriteTo.File("Logs/LogInformation.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
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
                                    new[] { "" }
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
            builder.Services.AddCaptcha(builder.Configuration, option =>
            {
                option.CaptchaType = CaptchaType.WORD; // 验证码类型
                option.CodeLength = 6; // 验证码长度, 要放在CaptchaType设置后.  当类型为算术表达式时，长度代表操作的个数
                option.ExpirySeconds = 30; // 验证码过期时间
                option.IgnoreCase = true; // 比较时是否忽略大小写
                option.StoreageKeyPrefix = "SpeakEase"; // 存储键前缀

                option.ImageOption.Animation = true; // 是否启用动画
                option.ImageOption.FrameDelay = 30; // 每帧延迟,Animation=true时有效, 默认30

                option.ImageOption.Width = 150; // 验证码宽度
                option.ImageOption.Height = 50; // 验证码高度
                option.ImageOption.BackgroundColor = SkiaSharp.SKColors.White; // 验证码背景色

                option.ImageOption.BubbleCount = 2; // 气泡数量
                option.ImageOption.BubbleMinRadius = 5; // 气泡最小半径
                option.ImageOption.BubbleMaxRadius = 15; // 气泡最大半径
                option.ImageOption.BubbleThickness = 1; // 气泡边沿厚度

                option.ImageOption.InterferenceLineCount = 2; // 干扰线数量

                option.ImageOption.FontSize = 36; // 字体大小
                option.ImageOption.FontFamily = DefaultFontFamilys.Instance.Actionj; // 字体

                /* 
                 * 中文使用kaiti，其他字符可根据喜好设置（可能部分转字符会出现绘制不出的情况）。
                 * 当验证码类型为“ARITHMETIC”时，不要使用“Ransom”字体。（运算符和等号绘制不出来）
                 */
            });
            #endregion

            #region SnowflakeId


            builder.Services.AddIdGen(123, () => new IdGeneratorOptions()); // Where 123 is the generator-id

            #endregion

            #region swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "SpeakEase v1",
                        Title = "SpeakEase API",
                        Description = "Web API for managing By Calo-YG",
                        TermsOfService = new Uri("https://gitee.com/wen-yaoguang"),
                        Contact = new OpenApiContact
                        {
                            Name = "Gitee 地址",
                            Url = new Uri("https://gitee.com/wen-yaoguang/Colo.Blog")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "个人博客",
                            Url = new Uri("https://www.se.cnblogs.com/lonely-wen/")
                        }
                    }
                );
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            });

            #endregion

            builder.Services.WithFast();

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            app.MapDefaultEndpoints();

            app.UseCors("cors");

            app.UseStaticFiles();

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = SerilogRequestUtility.HttpMessageTemplate;
                options.GetLevel = SerilogRequestUtility.GetRequestLevel;
                options.EnrichDiagnosticContext = SerilogRequestUtility.EnrichFromRequest;
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SpeakEase");
                    options.EnableDeepLinking();
                });
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapGet("SpeakEase/health", (IDbContext context) => Results.Ok("SpeakEase"));

            app.MapFast();
            
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