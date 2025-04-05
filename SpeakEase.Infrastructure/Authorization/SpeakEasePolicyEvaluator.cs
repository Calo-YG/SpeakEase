using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace SpeakEase.Infrastructure.Authorization
{
    public class SpeakEasePolicyEvaluator(IAuthorizationService authorizationService) : PolicyEvaluator(authorizationService), IPolicyEvaluator
    {

        public async override Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            ArgumentNullException.ThrowIfNull(policy);

            var result = await authorizationService.AuthorizeAsync(context.User, resource, policy);
            if (result.Succeeded)
            {
                return PolicyAuthorizationResult.Success();
            }

            // If authentication was successful, return forbidden, otherwise challenge
            return (authenticationResult.Succeeded)
                ? PolicyAuthorizationResult.Forbid(result.Failure)
                : PolicyAuthorizationResult.Challenge();
        }
    }
}
