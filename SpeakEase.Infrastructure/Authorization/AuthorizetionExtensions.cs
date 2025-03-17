using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Infrastructure.Authorization;

public static class AuthorizetionExtensions
{
      public static IServiceCollection RegisterAuthorizetion(this IServiceCollection services)
      {
            services.AddScoped<IPermissionCheck, PermissionCheck>();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationProvider>();
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizeMiddleHandle>();
            services.AddSingleton<IAuthorizationHandler, AuthorizeHandler>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<TokenManager>();
            return services;
      }
}