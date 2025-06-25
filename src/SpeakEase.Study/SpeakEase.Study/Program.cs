using IdGen;
using IdGen.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using SpeakEase.Infrastructure.Middleware;
using SpeakEase.Infrastructure.Options;
using SpeakEase.Study.Host.MapRoute;
using SpeakEase.Study.Infrastructure.EntityFrameworkCore;

namespace SpeakEase.Study.Host;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        #region configure serilogW

        Log.Logger = new LoggerConfiguration()
            .CreateLogger();

        #endregion

        try
        {
            Log.Information("Starting web host");

            var builder = WebApplication.CreateSlimBuilder(args);

            //builder.Host.UseAgileConfig(($"Configurations/agileconfig.json"));

            builder.Configuration.AddJsonFile("Configurations/redis.json", true, true)
                .AddJsonFile("Configurations/database.json", true, true)
                .AddJsonFile("Configurations/jwt.json", true, true)
                .AddJsonFile("Configurations/database.json", true, true)
                .AddJsonFile("Configurations/cors.json", true, true);   

            builder.Host.UseSerilog(
                (context, services, configuration) =>
                    configuration.ReadFrom
                        .Configuration(context.Configuration)
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .ReadFrom.Services(services)
                        .Enrich.FromLogContext()
                        .WriteTo.File("Logs/LogError.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error)
                        .WriteTo.File("Logs/LogWarning.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Warning)
                        .WriteTo.Console());

            builder.AddServiceDefaults();

            builder.Services.AddRouting();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddScoped<ExceptionMiddleware>();

            var configuration = builder.Configuration;

            var cors = configuration.GetSection("CorsOptions").Get<CorsOptions>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.RegisterEntityFrameworkCoreContext(builder.Configuration)
                .ConfigureJson()
                .ConfigureCors(configuration)
                .ConfigureJwt(configuration)
                .ConfigureRedis(configuration)
                //.RegisterLocalEventBus()
                .AddCaptcha(builder.Configuration)
                .AddIdGen(123, () => new IdGeneratorOptions())
                .AddOpenApi(options =>
                {

                })
                .BuilderApplication();

            var app = builder.Build();

            app.MapDefaultEndpoints();

            app.UseCors(cors.Policy);

            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference(options =>
                {
                    options.WithTitle("SpeakEase");
                    options.WithTheme(ScalarTheme.Moon);
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

            app.MapGet("SpeakEase/health", () => Results.Ok("SpeakEase"));


            app.MapAuthEndpoint();
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