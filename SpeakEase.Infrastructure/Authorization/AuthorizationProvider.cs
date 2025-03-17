using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace SpeakEase.Infrastructure.Authorization;

public class AuthorizationProvider(IAuthenticationSchemeProvider authenticationSchemeProvider):IAuthorizationPolicyProvider
{
      public async  Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
      {
            var policy = new AuthorizationPolicyBuilder();
            
            await SetScheme(policy);
            
            var authorizations = policyName.Split(',');
            
            if (authorizations.Any())
            {
                  policy.AddRequirements(new AuthorizeRequirement(authorizations));
            }

            return policy.Build();
      }

      public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
      {
            var policy = new AuthorizationPolicyBuilder();
            await SetScheme(policy);
            return policy.Build();
      }

      public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
      {
            var policy = new AuthorizationPolicyBuilder();
            await SetScheme(policy);
            return policy.Build();
      }

      private async Task SetScheme(AuthorizationPolicyBuilder policyBuilder)
      {
           var schemes =  await authenticationSchemeProvider.GetAllSchemesAsync();
           
             foreach (var scheme in schemes)
             {
                    policyBuilder.AuthenticationSchemes.Add(scheme.Name);
             }
      }
}