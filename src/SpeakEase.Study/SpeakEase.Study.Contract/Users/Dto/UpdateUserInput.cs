﻿namespace SpeakEase.Study.Contract.Users.Dto
{
    /// <summary>
    /// 更新用户信息
    /// </summary>
    public class UpdateUserInput
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }
    }
}
