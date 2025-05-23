using System;

namespace SpeakEase.FastEndpoint.Attributes
{
    /// <summary>
    /// AuthorizeAttribute
    /// </summary>
    /// <param name="authencationScheme"></param>
    /// <param name="policy"></param>
    public class AuthorizeAttribute(string authencationScheme,string policy) : Attribute
    {
        public string AuthenticationScheme { get; } = authencationScheme;
        public string Policy { get; } = policy;
    }
}
