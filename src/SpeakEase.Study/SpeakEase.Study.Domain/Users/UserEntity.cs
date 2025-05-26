using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.Users;

public class UserEntity:Entity<long>
{
    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; private set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public string UserAccount { get; private set; }

    /// <summary>
    /// 用户密码
    /// </summary>
    public string Password { get; private set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 用户手机
    /// </summary>
    public string Phone { get; private set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; private set; }

    /// <summary>
    /// 微信用户主键
    /// </summary>
    public string WeChatKey { get; private set; }

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// 修改事件
    /// </summary>
    public DateTime? ModifyAt { get; private set; }

    protected UserEntity()
    {
    }

    public UserEntity(long id, string userName, string userAccount, string userPassword, string email, string phone, string avatar)
    {
        Id = id;
        UserName = userName;
        UserAccount = userAccount;
        Password = userPassword;
        Email = email;
        Phone = phone;
        Avatar = avatar;
        CreatedAt = DateTime.Now;
        IsActive = true;
    }


    public void Modify(DateTime date,string password)
    {
        ModifyAt = date;
        Password = password;
    }

    /// <summary>
    /// 设置头像存储路径
    /// </summary>
    /// <param name="url"></param>
    public void SetAvatar(string url)
    {
        Avatar = url;
    }
}