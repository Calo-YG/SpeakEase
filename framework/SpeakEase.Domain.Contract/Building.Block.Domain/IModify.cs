namespace SpeakEase.Domain.Contract.Building.Block.Domain
{
    /// <summary>
    /// 修改
    /// </summary>
    public interface IModify
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string ModifyById { get; set; }

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
