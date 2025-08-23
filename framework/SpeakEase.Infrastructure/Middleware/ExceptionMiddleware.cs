using Microsoft.AspNetCore.Http;
using SpeakEase.Infrastructure.Exceptions;
using System.Text.Json;
using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Infrastructure.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
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
                await next(context);
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
            var response = Response.Fail(ex.Message, statusCode);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _options));
        }
    }
}
