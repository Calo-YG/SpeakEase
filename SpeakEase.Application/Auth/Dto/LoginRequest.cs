namespace SpeakEase.Contract.Auth.Dto
{
    /// <summary>
    /// loing request
    /// </summary>
    public sealed class LoginRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }
    }
}
