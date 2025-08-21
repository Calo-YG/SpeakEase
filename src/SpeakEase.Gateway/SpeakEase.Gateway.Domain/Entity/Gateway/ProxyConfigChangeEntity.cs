using SpeakEase.Domain.Contract.Building.Block.Domain;
using SpeakEase.Gateway.Domain.Enum;

namespace SpeakEase.Gateway.Domain.Entity.Gateway
{
    /// <summary>
    /// 代理更新配置实体
    /// </summary>
    public class ProxyConfigChangeEntity:ICreation
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 更新备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 更改枚举
        /// </summary>
        public ProxyConfigChageEnum ChageEnum { get; set; }

        /// <summary>
        /// 事件id
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新人Id
        /// </summary>
        public string CreateById { get; set; }

        /// <summary>
        /// 更新人名称
        /// </summary>
        public string CreateByName { get; set; }
    }
}
