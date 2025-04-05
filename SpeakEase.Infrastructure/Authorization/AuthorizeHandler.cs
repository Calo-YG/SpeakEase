using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SpeakEase.Infrastructure.SpeakEase.Core;

namespace SpeakEase.Infrastructure.Authorization;

public class AuthorizeHandler(
    IServiceProvider serviceProvider,
    IHttpContextAccessor contextAccessor) : AuthorizationHandler<AuthorizeRequirement>, IDisposable
{
    private readonly AsyncServiceScope scope = serviceProvider.CreateAsyncScope();

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeRequirement requirement)
    {
        var currentEndpoint = contextAccessor.HttpContext.GetEndpoint();

        var authorizeData = currentEndpoint.Metadata.GetMetadata<IAuthorizeData>();

        //默认授权策略
        if (authorizeData is null)
        {
            context.Succeed(requirement);
            return;
        }

        //如果没有授权策略，直接通过
        if (authorizeData.Policy.IsNullOrEmpty())
        {
            context.Succeed(requirement);
            return;
        }

        var currentUser = scope.ServiceProvider.GetRequiredService<IUserContext>();
        var permisscheck = scope.ServiceProvider.GetRequiredService<IPermissionCheck>();
        AuthorizationFailureReason failureReason;
        if (!currentUser.IsAuthenticated)
        {
            failureReason = new AuthorizationFailureReason(this, "Please log in to the system");
            context.Fail(failureReason);
            return;
        }

        if (!await permisscheck.IsGranted(currentUser.UserId!, authorizeData.Policy))
        {
            failureReason = new AuthorizationFailureReason(this,
                $"Insufficient permissions, unable to request - request interface{contextAccessor.HttpContext?.Request?.Path ?? string.Empty}");
            context.Fail(failureReason);
            return;
        }

        context.Succeed(requirement);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // TODO 在此释放托管资源
            scope.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
}