namespace SpeakEase.Study.Domain.Shared
{
    public interface IModify
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long ModifyById { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string ModifyByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ModifyAt { get; set; }
    }
}
