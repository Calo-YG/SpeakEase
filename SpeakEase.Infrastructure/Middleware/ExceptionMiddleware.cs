using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpeakEase.Infrastructure.Authorization;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Filters;
using System.Diagnostics;
using System.Text.Json;

namespace SpeakEase.Infrastructure.Middleware
{
    /// <summary>
    /// 异常中间件
    /// </summary>
    /// <param name="loggerFactory"></param>
    public class ExceptionMiddleware(ILoggerFactory loggerFactory, IOptions<JsonOptions> options) : IMiddleware
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger("ExceptionMiddleware");
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var path = context.Request.Path;


            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                await next(context);
                stopwatch.Stop();
                _logger.LogInformation($"{context.Request.Path}--duration: {stopwatch.ElapsedMilliseconds} 毫秒");
            }
            catch (UserFriednlyException ex)
            {
                await HandleExceptionAsync(context, ex, 499, options, _logger);
            }
            catch (AntiforgeryValidationException ex)
            {
                await HandleExceptionAsync(context, ex, 500, options, _logger);
            }
            catch(RefeshTokenValidateException ex)
            {
                await HandleExceptionAsync(context, ex, 999, options, _logger);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, 500, options, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, int statusCode, IOptions<JsonOptions> options, ILogger logger)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = Response.Fail(ex.Message, statusCode);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options.Value.SerializerOptions));

            logger.LogError(ex, $"Middleware Error: {ex.Message}");
        }
    }
}
