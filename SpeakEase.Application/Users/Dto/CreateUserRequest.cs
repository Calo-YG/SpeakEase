namespace SpeakEase.Contract.Users.Dto
{
    /// <summary>
    /// 创建用户请求
    /// </summary>
    public sealed class CreateUserRequest
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// 唯一键
        /// </summary>
        public string UnquneId { get; set; }
    }
}
