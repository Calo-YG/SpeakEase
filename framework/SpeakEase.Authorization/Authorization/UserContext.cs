using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SpeakEase.Infrastructure.Exceptions;

namespace SpeakEase.Authorization.Authorization;

public class UserContext : IUserContext
{
    private readonly IEnumerable<Claim> _claims;

    public bool IsAuthenticated { get; }
    public string UserId { get; private set; }

    public string UserName => _userName;

    private string _userName;
    public string UserAccount => _userAccount;

    private string _userAccount;
    public long? OrganizationId => _organizationId;

    private long? _organizationId;
    public string OrganizationName => _organizationName;

    private string _organizationName;

    public User User => GetUser();

    private User _user;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        var identity = httpContextAccessor.HttpContext?.User.Identity;
        _claims = httpContextAccessor.HttpContext?.User.Claims;
        IsAuthenticated = identity?.IsAuthenticated ?? false;
    }

    private User GetUser()
    {
        if(_user is not null)
        {
            return this._user;
        }
        SetUserId();
        SetUserName();
        SetUserAccount();
        this._user = new User(UserId, UserName, UserAccount);
        return this._user;
    }

    private void SetUserId()
    {
        var value = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserId)?.Value;

        if (string.IsNullOrEmpty(value))
        {
             ThrowUserFriendlyException.ThrowException("Can not get user claims info -- this UserId");
        }

        UserId = value;
    }

    private void SetOrganizationId()
    {
        var value = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.OrganizationId)?.Value;

        var parse = long.TryParse(value, out var result);

        _organizationId = result;
    }

    private void SetUserName()
    {
        _userName = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserName)?.Value;

        if (string.IsNullOrEmpty(_userName))
        {
            ThrowUserFriendlyException.ThrowException("Can not get user claims info -- this UserName");
        }
    }

    private void SetUserAccount()
    {
        _userAccount = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.UserAccount)?.Value;

        if (string.IsNullOrEmpty(_userAccount))
        {
            ThrowUserFriendlyException.ThrowException("Can not get user claims info -- this UserAccount");
        }
    }

    private void SetOrganizationName()
    {
        _organizationName = _claims?.FirstOrDefault(x => x.Type == UserInfomationConst.OrganizationName)?.Value;
    }
}