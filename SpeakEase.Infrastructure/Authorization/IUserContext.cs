namespace SpeakEase.Infrastructure.Authorization;

public interface IUserContext
{
    /// <summary>
    /// 是否通过鉴权
    /// </summary>
    public bool IsAuthenticated { get; }

    /// <summary>
    /// 用户id
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public string UserAccount { get; }

    /// <summary>
    /// 用户组织id
    /// </summary>
    public long? OrganizationId { get; }

    /// <summary>
    /// 用户组织名称
    /// </summary>
    public string? OrganizationName { get; }

    /// <summary>
    /// 当前登录用户信息
    /// </summary>
    public User User { get; }
}