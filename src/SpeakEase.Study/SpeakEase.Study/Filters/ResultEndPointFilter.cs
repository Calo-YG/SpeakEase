using Microsoft.AspNetCore.Http.HttpResults;

namespace SpeakEase.Infrastructure.Filters
{
    /// <summary>
    /// 结果过滤器
    /// </summary>
    public class ResultEndPointFilter : IEndpointFilter
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var result = await next(context);

            if(result is not FileStreamHttpResult)
            {
                return Response.Sucess(result);
            }

            return result;
        }
    }
}
