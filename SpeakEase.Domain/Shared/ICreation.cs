namespace SpeakEase.Domain.Shared
{
    /// <summary>
    /// 创建基类接口
    /// </summary>
    public interface ICreation
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long CreateById { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string CreateByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
