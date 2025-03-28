﻿namespace SpeakEase.Contract.Users.Dto
{
    /// <summary>
    /// 当前登录用户信息
    /// </summary>
    public sealed class UserResponse
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 用户手机
        /// </summary>
        public string Phone { get;  set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get;  set; }
    }
}
