using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SpeakEase.Gateway.Infrastructure.GatewayLog
{
    public class OperateLogEndpointFilter(ILogger<OperateLogEndpointFilter> logger): IEndpointFilter
    {
        /// <summary>
        /// 模板
        /// </summary>
        private readonly string _template = "\n 请求开始时间:{0} \n 模块名称：{1} \n 路由名称：{2} \n 请求路径：{3}\n 请求方式：{4} \n 耗时：{5} 毫秒 \n 异常信息：{6} \n 异常堆栈信息：{7} \n 请求结束时间：{8}";

        /// <summary>
        /// 开启时间
        /// </summary>
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            try
            {
                var result = await next(context);

                BuildLogMessage(context.HttpContext);

                return result;
            }
            catch (Exception ex)
            {
                BuildLogMessage(context.HttpContext, ex);
                throw;
            }
        }


        /// <summary>
        /// 构建信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <param name="statusCode">状态码</param>
        private void BuildLogMessage(HttpContext context, Exception ex = null)
        {
            _stopwatch.Stop();

            var endpoint = context!.GetEndpoint();

            var logdata = endpoint!.Metadata.GetMetadata<ILogOperate>();

            var module = logdata?.Module;

            var routename = logdata?.RouteName;

            var message = string.Format(_template, DateTime.Now,module ?? string.Empty,routename ?? string.Empty, context.Request?.Path, context.Request?.Method, _stopwatch.ElapsedMilliseconds, ex?.Message,ex?.StackTrace, DateTime.Now);

            if(ex == null)
            {
                logger.LogInformation(message);
            }
            else
            {
                logger.LogError(message);
            }

            if(logdata?.WriteDatabase ?? false)
            {
                // todo async record database
            }
        }
    }
}
