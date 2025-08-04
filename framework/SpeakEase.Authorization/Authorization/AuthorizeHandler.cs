using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SpeakEase.Infrastructure.Redis;

namespace SpeakEase.Authorization.Authorization;

public class AuthorizeHandler(
    IServiceProvider serviceProvider,
    IHttpContextAccessor contextAccessor,IRedisService redisService) : AuthorizationHandler<AuthorizeRequirement>
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

        var allowAnonymouse = currentEndpoint.Metadata.GetMetadata<IAllowAnonymous>();
        var authorizeData = currentEndpoint.Metadata.GetMetadata<IAuthorizeData>();

        // 如果是匿名访问，直接通过
        if (allowAnonymouse is not null || authorizeData is null)
        {
            context.Succeed(requirement);
            
            return;
        }
        
        if (!context.User.Identity!.IsAuthenticated)
        {
            failureReason = new AuthorizationFailureReason(this, "Unauthorized, please login first");

            context.Fail(failureReason);

            return;
        }
        
        await using var scope = serviceProvider.CreateAsyncScope();

        var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();
        
        var user = userContext.User;

        var existToken = await redisService.ExistsAsync(string.Format(UserInfomationConst.RedisTokenKey, user.Id));
        
        if (!existToken)
        {
            failureReason = new AuthorizationFailureReason(this, "Token expired, please login again");

            context.Fail(failureReason);

            return;
        }

        //无授权策略
        if (string.IsNullOrEmpty(authorizeData?.Policy))
        {
            context.Succeed(requirement);
            
            return;
        }

        var permissionCheck = scope.ServiceProvider.GetRequiredService<IPermissionCheck>();

        if (!await permissionCheck.IsGranted(userContext.UserId!, authorizeData.Policy))
        {
            failureReason = new AuthorizationFailureReason(this,
                $"Insufficient permissions, unable to request - request interface{contextAccessor.HttpContext?.Request?.Path ?? string.Empty}");

            context.Fail(failureReason);
        }
        else
        {
            context.Succeed(requirement);
        }
    }
}