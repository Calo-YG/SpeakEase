using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using SpeakEase.Infrastructure.SpeakEase.Core;

namespace SpeakEase.Infrastructure.Authorization;

public class CurrentUser:ICurrentUser, ITransientDependency
{
      private readonly IHttpContextAccessor _httpContextAccessor;
      private readonly IIdentity? _identity;
      private readonly IEnumerable<Claim>? _claims;
      
      public bool IsAuthenticated { get; }
      public string UserId { get; }
      public string UserName { get; }
      public string UserAccount { get; }
      public string OrganizationId { get; }
      public string OrganizationName { get; }
      
      public User User { get; }


      public CurrentUser(IHttpContextAccessor httpContextAccessor)
      {
            _httpContextAccessor = httpContextAccessor;
            _identity = _httpContextAccessor.HttpContext?.User?.Identity;
            _claims = httpContextAccessor.HttpContext?.User?.Claims;
            IsAuthenticated = _identity?.IsAuthenticated ?? false;
            UserId = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserId)?.Value ?? null;
            UserName = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserName)?.Value ?? null;
            UserAccount = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserAccount)?.Value ?? null;
            OrganizationId = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.OrganizationId)?.Value ?? null;
            OrganizationName = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.OrganizationName)?.Value ?? null;
            User = new User(UserId,UserName,UserAccount,OrganizationId,OrganizationName);
      }
}