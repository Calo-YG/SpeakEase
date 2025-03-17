﻿using System.ComponentModel.DataAnnotations.Schema;

namespace SpeakEase.Domain.Users;

[Table("refresh_token")]
public class RefreshTokenEntity
{
      /// <summary>
      /// 主键
      /// </summary>
      public long Id { get;private set; }
      
      /// <summary>
      /// 用户主键
      /// </summary>
      public string UserId { get;private set; }
      
      /// <summary>
      /// 当前token
      /// </summary>
      public string Token { get;private set; }
      
      /// <summary>
      /// 过期时间
      /// </summary>
      public DateTimeOffset Expires { get;private set; }
      
      /// <summary>
      /// 是否被使用
      /// </summary>
      public bool IsUsed { get;private set; }
      
      /// <summary>
      /// 创建时间
      /// </summary>
      public DateTimeOffset CreationTime { get;private set; }
      
      protected RefreshTokenEntity()
      {
      }

      public RefreshTokenEntity(int id, string userId, string token, DateTimeOffset expires, bool isUsed)
      {
            Id = id;
            UserId = userId;
            Token = token;
            Expires = expires;
            IsUsed = isUsed;
      }
}