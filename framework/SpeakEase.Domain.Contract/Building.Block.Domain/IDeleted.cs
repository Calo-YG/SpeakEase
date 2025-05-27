namespace SpeakEase.Domain.Contract.Building.Block.Domain
{
    /// <summary>
    /// 删除
    /// </summary>
    public interface IDeleted
    {
        /// <summary>
        /// 删除
        /// </summary>
        public string DeletedById { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string DeletedByName { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 删除日期      
        /// </summary>
        public DateTime? DeleteDate { get; set; }
    }
}
