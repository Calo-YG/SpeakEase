namespace SpeakEase.Domain.Shared
{
    public interface IModify
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long ModifyUserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ModifyAt { get; set; }
    }
}
