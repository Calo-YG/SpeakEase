using Microsoft.AspNetCore.Authorization;

namespace SpeakEase.Authorization.Authorization;

public class AuthorizeRequirement(params string[] authorizeName) : IAuthorizationRequirement
{
    public  string[] AuthorizeName { get; private set; } = authorizeName;
}