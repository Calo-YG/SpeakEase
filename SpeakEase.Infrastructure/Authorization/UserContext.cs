using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.SpeakEase.Core;

namespace SpeakEase.Infrastructure.Authorization;

public class UserContext : IUserContext, ITransientDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IIdentity _identity;
    private readonly IEnumerable<Claim> _claims;

    public bool IsAuthenticated { get; }
    public long UserId { get => _userId; }

    private long _userId;
    public string UserName { get => _userName; }

    public string _userName;
    public string UserAccount { get => _userAccount; }

    private string _userAccount;
    public long OrganizationId { get => _organizationId; }

    private long _organizationId;
    public string OrganizationName { get; }

    private string _organizationName;

    public User User { get; }


    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _identity = _httpContextAccessor.HttpContext?.User?.Identity;
        _claims = httpContextAccessor.HttpContext?.User?.Claims;
        IsAuthenticated = _identity?.IsAuthenticated ?? false;
        SetUserId();
        SetOrganizationId();
        SetUserName();
        SetUserAccount();
        SetOrganizationName();
        User = new User(UserId, UserName, UserAccount, OrganizationId, OrganizationName);
    }

    private void SetUserId()
    {
        var value = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserId)?.Value ?? null;

        if (string.IsNullOrEmpty(value))
        {
             ThrowUserFriendlyException.ThrowException("Can not get user claims info");
        }

        var tryget = long.TryParse(value, out var result);

        if (tryget)
        {
            ThrowUserFriendlyException.ThrowException("try get user claims info error");
        }

        _userId = result;
    }

    private void SetOrganizationId()
    {
        var value = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.OrganizationId)?.Value ?? null;

        if (string.IsNullOrEmpty(value))
        {
            ThrowUserFriendlyException.ThrowException("Can not get user claims info");
        }

        var tryget = long.TryParse(value, out var result);

        if (tryget)
        {
            ThrowUserFriendlyException.ThrowException("try get user claims info error");
        }

        _organizationId = result;
    }

    private void SetUserName()
    {
        _userName = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserName)?.Value ?? null;

        if (!string.IsNullOrEmpty(_userName))
        {
            ThrowUserFriendlyException.ThrowException("Can not get user claims info");
        }
    }

    private void SetUserAccount()
    {
        _userAccount = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserAccount)?.Value ?? null;

        if (!string.IsNullOrEmpty(_userName))
        {
            ThrowUserFriendlyException.ThrowException("Can not get user claims info");
        }
    }

    private void SetOrganizationName()
    {
        _organizationName = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.OrganizationName)?.Value ?? null;

        if (!string.IsNullOrEmpty(_userName))
        {
            ThrowUserFriendlyException.ThrowException("Can not get user claims info");
        }
    }
}