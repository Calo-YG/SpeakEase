namespace SpeakEase.Study.Contract.Auth.Dto
{
    /// <summary>
    /// 验证码响应
    /// </summary>
    public sealed class VerificationCodeDto
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        public string UniqueId { get; set; }

        /// <summary>
        /// 验证吗
        /// </summary>
        public string VerificationCode { get; set; }
    }
}
