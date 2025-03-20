using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Infrastructure.Authorization;

public class AuthorizeMiddleHandle(ILoggerFactory factory, IOptions<JsonOptions> options) : IAuthorizationMiddlewareResultHandler
{
    private readonly ILogger _logger = factory.CreateLogger<IAuthorizationMiddlewareResultHandler>();

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if ((!authorizeResult.Succeeded || authorizeResult.Challenged))
        {
            var issAuthenticated = context.User?.Identity?.IsAuthenticated ?? false;

            var reason = string.Join(",",
                authorizeResult.AuthorizationFailure!.FailureReasons!
                    .Where(p => !string.IsNullOrEmpty(p.Message))
                    .Select(p => p.Message!));

            _logger.LogWarning($"Authorization failed  with reason: {reason}");

            var errorcode = issAuthenticated ? 500 : 401;

            context!.Response.StatusCode = errorcode;
            context.Response.ContentType = "application/json";
            var response = new Response<string>(false, reason, errorcode);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options.Value.SerializerOptions));
            return;
        }

        await next(context);
    }
}