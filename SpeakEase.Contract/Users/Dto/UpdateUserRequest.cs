namespace SpeakEase.Contract.Users.Dto
{
    /// <summary>
    /// 更新用户信息
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId  { get; set; }

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
