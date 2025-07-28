using SpeakEase.Domain.Contract.Entity;

namespace SpeakEase.Gateway.Domain.Entity.System;

public class SysUserEntity:SpeakEaseEntity
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
    
    protected SysUserEntity()
    { 
    }
    
    /// <summary>
    /// 创建用户
    /// </summary>
   /// <param name="id"></param>
    /// <param name="account"></param>
    /// <param name="password"></param>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="avatar"></param>
    public SysUserEntity(string id,string account, string password, string name, string email, string avatar)
    {
        Id = id;
        Account = account;
        Password = password;
        Name = name;
        Email = email;
        Avatar = avatar;
    }

    /// <summary>
    /// 设置用户名称
    /// </summary>
    /// <param name="name"></param>
    public void SetUserName(string name)
    {
        Name = name;
    }
    
    /// <summary>
    /// 设置用户邮箱
    /// </summary>
    /// <param name="email"></param>
    public void SetUserEmail(string email)
    {
        Email = email;
    }
    
    /// <summary>
    /// 设置用户头像
    /// </summary>
    /// <param name="avatar"></param>
    public void SetUserAvatar(string avatar)
    {
        Avatar = avatar;
    }
    
    /// <summary>
    /// 设置用户密码
    /// </summary>
    /// <param name="password"></param>
    public void SetUserPassword(string password)
    {
        Password = password;
    }
}