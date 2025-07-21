using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SpeakEase.Infrastructure.Exceptions;
using System.Diagnostics;
using System.Text.Json;
using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Infrastructure.Middleware
{
    /// <summary>
    /// 异常中间件
    /// </summary>
    /// <param name="loggerFactory"></param>
    public class ExceptionMiddleware(ILoggerFactory loggerFactory) : IMiddleware
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger("ExceptionMiddleware");

        /// <summary>
        /// 模板
        /// </summary>
        private readonly string _template = "\n 请求开始时间:{0}\n 请求路径：{1}\n 请求方式：{2} \n 耗时：{3} 毫秒 \n 异常：{4}\n 响应状态码：{5} \n 请求结束时间：{6}";
 
        /// <summary>
        /// 开启时间
        /// </summary>
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        /// <summary>
        /// 配置序列化
        /// </summary>
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        /// <summary>
        /// 执行中间处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
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
            catch (UserFriendlyException ex)
            {
                await HandleExceptionAsync(context, ex, 499);
            }
            catch (RefreshTokenValidateException ex)
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
            BuildLogMessage(context, ex, statusCode);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _options));
        }

        /// <summary>
        /// 构建信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <param name="statusCode">状态码</param>
        private void BuildLogMessage(HttpContext context, Exception ex = null, int statusCode = 200)
        {
            var message = string.Format(_template, DateTime.Now,context.Request?.Path, context.Request?.Method, _stopwatch.ElapsedMilliseconds, ex?.StackTrace, statusCode, DateTime.Now);

            _logger.LogInformation(message);
        }
    }
}
