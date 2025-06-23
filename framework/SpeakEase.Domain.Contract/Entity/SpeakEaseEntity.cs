using SpeakEase.Domain.Contract.Building.Block.Domain;

namespace SpeakEase.Domain.Contract.Entity
{
    /// <summary>
    /// 基类实体
    /// </summary>
    public abstract class SpeakEaseEntity:Entity<string>,ICreation,IModify
    {
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateById { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateByName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyById { get; set; }
        /// <summary>
        /// 修改人主键
        /// </summary>
        public string ModifyByName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyAt { get; set; }
    }
}
