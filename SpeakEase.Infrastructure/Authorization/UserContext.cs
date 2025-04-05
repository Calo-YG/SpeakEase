using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using SpeakEase.Infrastructure.Exceptions;

namespace SpeakEase.Infrastructure.Authorization;

public class UserContext : IUserContext
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
    public long? OrganizationId { get => _organizationId; }

    private long? _organizationId;
    public string OrganizationName { get => _organizationName; }

    private string _organizationName;

    public User User { get => GetUser(); }

    private User _user;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _identity = _httpContextAccessor.HttpContext?.User?.Identity;
        _claims = httpContextAccessor.HttpContext?.User?.Claims;
        IsAuthenticated = _identity?.IsAuthenticated ?? false;
    }

    private User GetUser()
    {
        if(_user is not null)
        {
            return this._user;
        }

        SetUserId();
        SetOrganizationId();
        SetUserName();
        SetUserAccount();
        SetOrganizationName();
        this._user = new User(UserId, UserName, UserAccount, OrganizationId, OrganizationName);
        return this._user;
    }

    private void SetUserId()
    {
        var value = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserId)?.Value ?? null;

        if (string.IsNullOrEmpty(value))
        {
             ThrowUserFriendlyException.ThrowException("Can not get user claims info -- this UserId");
        }

        var tryget = long.TryParse(value, out var result);

        if (!tryget)
        {
            ThrowUserFriendlyException.ThrowException("try get user claims info error -- this UserId");
        }

        _userId = result;
    }

    private void SetOrganizationId()
    {
        var value = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.OrganizationId)?.Value ?? null;

        var tryget = long.TryParse(value, out var result);

        _organizationId = result;
    }

    private void SetUserName()
    {
        _userName = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserName)?.Value ?? null;

        if (string.IsNullOrEmpty(_userName))
        {
            ThrowUserFriendlyException.ThrowException("Can not get user claims info -- this UserName");
        }
    }

    private void SetUserAccount()
    {
        _userAccount = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserAccount)?.Value ?? null;

        if (string.IsNullOrEmpty(_userAccount))
        {
            ThrowUserFriendlyException.ThrowException("Can not get user claims info -- this UserAccount");
        }
    }

    private void SetOrganizationName()
    {
        _organizationName = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.OrganizationName)?.Value ?? null;
    }
}