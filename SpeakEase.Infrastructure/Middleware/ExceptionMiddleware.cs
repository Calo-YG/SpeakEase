using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Shared;
using System.Diagnostics;

namespace SpeakEase.Infrastructure.Middleware
{
    /// <summary>
    /// 异常中间件
    /// </summary>
    /// <param name="loggerFactory"></param>
    public class ExceptionMiddleware(ILoggerFactory loggerFactory) : IMiddleware
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
                context!.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var response = new Response<string>(false, ex.Message, 500);
                _logger.LogError($"Middleware Error:{ex.Message}");
            }
            catch (AntiforgeryValidationException ex)
            {
                context!.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var response = new Response<string>(false, ex.Message, 500);
                _logger.LogError($"Middleware Error:{ex.Message}");
            }
        }
    }
}
