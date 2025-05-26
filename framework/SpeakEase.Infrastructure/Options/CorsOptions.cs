namespace SpeakEase.Infrastructure.Options
{
    /// <summary>
    /// 跨域配置
    /// </summary>
    public class CorsOptions
    {
        /// <summary>
        /// 允许的域名
        /// </summary>
        public string Origins { get; set; }

        /// <summary>
        /// 跨域名称
        /// </summary>
        public string Policy   { get; set; }
    }
}
