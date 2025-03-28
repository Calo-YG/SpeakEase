namespace SpeakEase.Domain.Users
{
    public class UserConst
    {
        /// <summary>
        /// token 缓存key
        /// </summary>
        public const string TokenCacheKey = "TokenCache_{0}";

        /// <summary>
        /// 注册验证码
        /// </summary>
        public const string RegiesCapchaCache = "RegiesCapchaCache_{0}";

        /// <summary>
        /// 登录验证码
        /// </summary>
        public const string LoginCapcahCache = "LoginCapcahCache_{0}";

        /// <summary>
        /// 更新用户验证
        /// </summary>
        public const string ModifyUserCapchaCache = "ModifyUserCapchaCache_{0}";
    }
}
