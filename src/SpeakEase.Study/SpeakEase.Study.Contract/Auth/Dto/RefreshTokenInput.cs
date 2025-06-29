﻿namespace SpeakEase.Study.Contract.Auth.Dto
{
    /// <summary>
    /// token
    /// </summary>
    public sealed class RefreshTokenInput
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string RefresherToken { get; set; }
    }
}
