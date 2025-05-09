using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Infrastructure.Authorization;

public static class AuthorizetionExtensions
{
      public static IServiceCollection RegisterAuthorizetion(this IServiceCollection services)
      {
            services.AddScoped<IPermissionCheck, PermissionCheck>();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizeResultHandle>();
            services.AddSingleton<IAuthorizationHandler, AuthorizeHandler>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<ITokenManager,TokenManager>();
            return services;
      }
}