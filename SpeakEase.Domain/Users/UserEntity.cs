using System.ComponentModel.DataAnnotations.Schema;
using SpeakEase.Domain.Users;
using SpeakEase.Domain.Users.Shared;

namespace SpeakEase.Domain.Users;

[Table("application_user")]
public class UserEntity
{
      protected UserEntity()
      {
            
      }
      
      public UserEntity(string id, string userName, string userAccount, string userPassword, string? email, string? phone, string avatar, SourceEnum source, string? weChatKey)
      {
            Id = id;
            UserName = userName;
            UserAccount = userAccount;
            UserPassword = userPassword;
            Email = email;
            Phone = phone;
            Avatar = avatar;
            Source = source;
            CreationTime = DateTimeOffset.Now;
            WeChatKey = weChatKey;
      }

      /// <summary>
      /// 用户id
      /// </summary>
      public string Id { get;private set; }
      
      /// <summary>
      /// 用户名称
      /// </summary>
      public string UserName { get;private set; }
      
      /// <summary>
      /// 用户账号
      /// </summary>
      public string UserAccount { get;private set; }
      
      /// <summary>
      /// 用户密码
      /// </summary>
      public string UserPassword { get;private set; }
      
      /// <summary>
      /// 邮箱
      /// </summary>
      public string? Email { get; set; }  
      
      /// <summary>
      /// 用户手机
      /// </summary>
      public string? Phone { get;private set; }
      
      /// <summary>
      /// 头像
      /// </summary>
      public string Avatar { get;private set; }
      
      /// <summary>
      /// 创建时间
      /// </summary>
      public DateTimeOffset CreationTime { get;private set; } 
      
      /// <summary>
      ///  用户来源
      /// </summary>
      public SourceEnum Source { get;private set; }
      
      /// <summary>
      /// 微信用户主键
      /// </summary>
      public string? WeChatKey { get;private set; }
}