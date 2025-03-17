using Microsoft.AspNetCore.Authorization;
using SpeakEase.Infrastructure.Exceptions;

namespace SpeakEase.Infrastructure.Authorization;

[AttributeUsage(AttributeTargets.Method)]
public class AuthorizationAttribute:AuthorizeAttribute
{
      public  string[] AuthorizeName { get; private set; }

      public AuthorizationAttribute(params string[]? authorizeName)
      {
            if(authorizeName == null || authorizeName.Length == 0)
            {
                  ThrowUserFriendlyException.ThrowException("no policy name for method");
            }
            
            AuthorizeName = authorizeName!;
            
            Policy = string.Join(",", AuthorizeName!);
      }
}