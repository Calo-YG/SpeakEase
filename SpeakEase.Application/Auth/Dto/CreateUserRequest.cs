namespace SpeakEase.Contract.Auth.Dto
{
    /// <summary>
    /// 创建用户请求
    /// </summary>
    public sealed class CreateUserRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }
    }
}
