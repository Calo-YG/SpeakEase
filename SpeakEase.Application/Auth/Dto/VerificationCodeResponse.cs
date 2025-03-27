namespace SpeakEase.Contract.Auth.Dto
{
    /// <summary>
    /// 验证码响应
    /// </summary>
    public sealed class VerificationCodeResponse
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        public long UniqueId { get; set; }

        /// <summary>
        /// 验证吗
        /// </summary>
        public string VerificationCode { get; set; }
    }
}
