using SpeakEase.Domain.Contract.Building.Block.Domain;
using SpeakEase.Domain.Contract.Entity;
using SpeakEase.Gateway.Domain.Enum;

namespace SpeakEase.Gateway.Domain.Entity.Gateway
{
    /// <summary>
    /// 节点实体
    /// </summary>
    public class DestinationEntity:Entity<string>,ICreation,IModify
    {
        /// <summary>
        /// 集群id
        /// </summary>
        public string ClusterId { get;private set; }

        /// <summary>
        /// 节点地址
        /// </summary>
        public string Address { get;private set; }

        /// <summary>
        /// 节点状态枚举
        /// </summary>
        public DestinationStateEnum State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateById { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateByName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ModifyById { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ModifyByName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyAt { get; set ; }

        protected DestinationEntity() { }

        public DestinationEntity(string clusterId, string address, DestinationStateEnum state)
        {
            ClusterId = clusterId;
            Address = address;
            State = state;
        }

        public void SetState(DestinationStateEnum state)
        {
            State = state;
        }
    }
}
