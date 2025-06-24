using SpeakEase.Domain.Contract.Entity;

namespace SpeakEase.Gateway.Domain.Entity.System;

public class SysUser:SpeakEaseEntity
{
    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get;private set; }
    
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get;private set; }
    
    /// <summary>
    /// 用户名称
    /// </summary>
    public string Name { get;private set; }
    
    /// <summary>
    /// 用户头像
    /// </summary>
    public string Email { get;private set; }
    
    /// <summary>
    /// 用户头像
    /// </summary>
    public string Avatar { get;private set; }
    
    protected SysUser()
    { 
    }
    
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="account"></param>
    /// <param name="password"></param>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="avatar"></param>
    public SysUser(string account, string password, string name, string email, string avatar)
    {
        Account = account;
        Password = password;
        Name = name;
        Email = email;
        Avatar = avatar;
    }
}