using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Infrastructure.Authorization;

public class AuthorizeHandler(
    IServiceProvider serviceProvider,
    IHttpContextAccessor contextAccessor) : AuthorizationHandler<AuthorizeRequirement>, IDisposable
{
    private readonly AsyncServiceScope _scope = serviceProvider.CreateAsyncScope();

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeRequirement requirement)
    {
        var currentUser = _scope.ServiceProvider.GetRequiredService<ICurrentUser>();
        var permisscheck = _scope.ServiceProvider.GetRequiredService<IPermissionCheck>();
        AuthorizationFailureReason failureReason;
        if (!currentUser.IsAuthenticated)
        {
            failureReason = new AuthorizationFailureReason(this, "Please log in to the system");
            context.Fail(failureReason);
            return;
        }

        var defaultPolicy = requirement.AuthorizeName?.Any() ?? false;
        //默认授权策略
        if (!defaultPolicy)
        {
            context.Succeed(requirement);
            return;
        }

        if (defaultPolicy && !await permisscheck.IsGranted(currentUser.UserId!, requirement.AuthorizeName!))
        {
            failureReason = new AuthorizationFailureReason(this,
                $"Insufficient permissions, unable to request - request interface{contextAccessor.HttpContext?.Request?.Path ?? ""}");
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
            _scope.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
}