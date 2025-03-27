namespace SpeakEase.Contract.Auth.Dto
{
    /// <summary>
    /// 登录返回token 响应
    /// </summary>
    public sealed class TokenResponse
    {
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }
    }
}
