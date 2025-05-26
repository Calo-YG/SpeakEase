namespace SpeakEase.Study.Contract.Auth.Dto
{
    /// <summary>
    /// 登录返回token 响应
    /// </summary>
    public sealed class TokenDto
    {
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
