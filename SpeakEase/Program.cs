using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using IdGen;
using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using SpeakEase.Domain;
using SpeakEase.Infrastructure.Authorization;
using SpeakEase.Infrastructure.EventBus;
using SpeakEase.Infrastructure.Shared;
using SpeakEase.Infrastructure.SpeakEase.Core;

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

            builder.AddServiceDefaults();

            builder.Services.AddEndpointsApiExplorer();
            

            var cors = "SpeakEase";

            var configuration = builder.Configuration;

            builder.Services.AddHttpContextAccessor();
            builder.Services.RegisterEntityFrameworkCoreContext(configuration);

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

            #endregion

            #region configure localeventbus

            builder.Services.RegisterLocalEventBus();

            #endregion

            #region SnowflakeId

            
            builder.Services.AddIdGen(123, () => new IdGeneratorOptions()); // Where 123 is the generator-id

            #endregion

            var app = builder.Build();

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
                //app.MapScalarApiReference(); // scalar/v1
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGet("speakease/health", () => Results.Ok("SpeakEase")).ProducesValidationProblem();

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