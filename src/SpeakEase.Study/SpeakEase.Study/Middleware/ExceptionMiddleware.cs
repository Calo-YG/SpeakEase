using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
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
    /// <param name="options"></param>
    public class ExceptionMiddleware(ILoggerFactory loggerFactory, IOptions<JsonOptions> options) : IMiddleware
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger("ExceptionMiddleware");

        /// <summary>
        /// 模板
        /// </summary>
        private readonly string Template = "\n-----------请求Start----------\n 请求路径：{0}\n 请求方式：{1} \n 耗时：{2} 毫秒 \n 异常：{3}\n 响应状态码：{4} \n-----------请求end----------";

        /// <summary>
        /// 开启时间
        /// </summary>
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var path = context.Request.Path;
                await next(context);
                _stopwatch.Stop();
                var duration = _stopwatch.ElapsedMilliseconds;
                BuildLogMessage(context);
            }
            catch (UserFriednlyException ex)
            {
                await HandleExceptionAsync(context, ex, 499);
            }
            catch(RefeshTokenValidateException ex)
            {
                await HandleExceptionAsync(context, ex, 999);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, 500);
            }
            
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            _stopwatch.Stop();
            var duration = _stopwatch.ElapsedMilliseconds;
            var response = Response.Fail(ex.Message, statusCode);
            BuildLogMessage(context,ex,statusCode);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options.Value.SerializerOptions));
        }

        /// <summary>
        /// 构建信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        private void BuildLogMessage(HttpContext context,Exception ex = null,int statusCode = 200)
        {
            var message = string.Format(Template, context.Request?.Path, context.Request?.Method, _stopwatch.ElapsedMilliseconds, ex?.StackTrace,statusCode);

            _logger.LogInformation(message);
        }
    }
}
