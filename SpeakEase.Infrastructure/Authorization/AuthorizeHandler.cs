﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Infrastructure.Authorization;

public class AuthorizeHandler(
    IServiceProvider serviceProvider,
    IHttpContextAccessor contextAccessor) : AuthorizationHandler<AuthorizeRequirement>, IDisposable
{
    private readonly AsyncServiceScope scope = serviceProvider.CreateAsyncScope();

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeRequirement requirement)
    {
        AuthorizationFailureReason failureReason;

        var currentEndpoint = contextAccessor.HttpContext.GetEndpoint();

        if(currentEndpoint is null)
        {
            failureReason = new AuthorizationFailureReason(this, "非法路由，What are you doing ?");

            context.Fail(failureReason);

            return;
        }

        var allowanymouse = currentEndpoint.Metadata.GetMetadata<IAllowAnonymous>();

        // 如果是匿名访问，直接通过
        if (allowanymouse is not null)
        {
            context.Succeed(requirement);
            return;
        }

        var authorizeData = currentEndpoint.Metadata.GetMetadata<IAuthorizeData>();

        //无授权策略
        if (authorizeData is null)
        {
            context.Succeed(requirement);
            return;
        }

        var currentUser = scope.ServiceProvider.GetRequiredService<IUserContext>();

        var permisscheck = scope.ServiceProvider.GetRequiredService<IPermissionCheck>();

        if (!currentUser.IsAuthenticated)
        {
            context.Fail();

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