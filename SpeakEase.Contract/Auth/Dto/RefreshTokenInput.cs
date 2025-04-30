namespace SpeakEase.Contract.Auth.Dto
{
    /// <summary>
    /// token
    /// </summary>
    public sealed class RefreshTokenInput
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string RefresherToken { get; set; }
    }
}
