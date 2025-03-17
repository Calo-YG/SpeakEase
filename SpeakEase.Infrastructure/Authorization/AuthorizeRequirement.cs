using Microsoft.AspNetCore.Authorization;

namespace SpeakEase.Infrastructure.Authorization;

public class AuthorizeRequirement : IAuthorizationRequirement
{
    public virtual string[] AuthorizeName { get; private set; }
    public AuthorizeRequirement(params string[] authorizeName)
    {
        AuthorizeName = authorizeName;
    }
}