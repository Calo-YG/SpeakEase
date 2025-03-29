using System.ComponentModel.DataAnnotations.Schema;
using SpeakEase.Domain.Users.Enum;

namespace SpeakEase.Domain.Users;

[Table("user")]
public class UserEntity
{
    protected UserEntity()
    {
    }

    public UserEntity(long id, string userName, string userAccount, string userPassword, string email, string phone, string avatar, SourceEnum source)
    {
        Id = id;
        UserName = userName;
        UserAccount = userAccount;
        UserPassword = userPassword;
        Email = email;
        Phone = phone;
        Avatar = avatar;
        Source = source;
        CreationTime = DateTime.Now;
    }

    /// <summary>
    /// 用户id
    /// </summary>
    public long Id { get; private set; }

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
    public string UserPassword { get; private set; }

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
    ///  用户来源
    /// </summary>
    public SourceEnum Source { get; private set; }

    /// <summary>
    /// 微信用户主键
    /// </summary>
    public string WeChatKey { get; private set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; private set; }

    /// <summary>
    /// 修改事件
    /// </summary>
    public DateTime? ModifyAt { get; private set; }


    public void Modify(DateTime date,string password)
    {
        ModifyAt = date;
        UserPassword = password;
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