using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Infrastructure.Filters
{
    public class ResultEndPointFilter : IEndpointFilter
    {
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var result = await next(context);

            if(result is not FileStreamHttpResult)
            {
                return Result.Success(result);
            }

            return result;
        }
    }
}
