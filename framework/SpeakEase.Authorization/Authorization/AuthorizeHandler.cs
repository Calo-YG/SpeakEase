using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Authorization.Authorization;

public class AuthorizeHandler(
    IServiceProvider serviceProvider,
    IHttpContextAccessor contextAccessor) : AuthorizationHandler<AuthorizeRequirement>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeRequirement requirement)
    {
        AuthorizationFailureReason failureReason;

        var currentEndpoint = contextAccessor.HttpContext!.GetEndpoint();

        if(currentEndpoint is null)
        {
            failureReason = new AuthorizationFailureReason(this, "非法路由，What are you doing ?");

            context.Fail(failureReason);

            return;
        }

        var allowAnonymousee = currentEndpoint.Metadata.GetMetadata<IAllowAnonymous>();

        // 如果是匿名访问，直接通过
        if (allowAnonymousee is not null)
        {
            context.Succeed(requirement);
            return;
        }

        var authorizeData = currentEndpoint.Metadata.GetMetadata<IAuthorizeData>();

        //无授权策略
        if (string.IsNullOrEmpty(authorizeData?.Policy))
        {
            context.Succeed(requirement);
            return;
        }

        await using var scope = serviceProvider.CreateAsyncScope();

        var currentUser = scope.ServiceProvider.GetRequiredService<IUserContext>();

        var permissionCheck = scope.ServiceProvider.GetRequiredService<IPermissionCheck>();

        if (!currentUser.IsAuthenticated)
        {
            context.Fail();

            return;
        }

        if (!await permissionCheck.IsGranted(currentUser.UserId!, authorizeData.Policy))
        {
            failureReason = new AuthorizationFailureReason(this,
                $"Insufficient permissions, unable to request - request interface{contextAccessor.HttpContext?.Request?.Path ?? string.Empty}");

            context.Fail(failureReason);

            return;
        }

        context.Succeed(requirement);
    }
}