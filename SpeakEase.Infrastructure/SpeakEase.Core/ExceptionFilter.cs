using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Infrastructure.SpeakEase.Core;

public class ExceptionFilter(IOptions<JsonOptions> options) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return next(context);
        }
        catch (Exception ex)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.HttpContext.Response.ContentType = "application/json";
            var response = new Response<string>(false, ex.Message, 500);
            await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(response, options.Value.SerializerOptions));
            return response;
        }
    }
}