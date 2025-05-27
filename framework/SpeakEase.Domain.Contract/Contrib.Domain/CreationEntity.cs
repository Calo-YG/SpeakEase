using SpeakEase.Domain.Contract.Building.Block.Domain;
using SpeakEase.Domain.Contract.Entity;

namespace SpeakEase.Domain.Contract.Contrib.Domain
{
    public class CreationEntity : Entity<string>, ICreation
    {
        /// <summary>
        /// 创建人id
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
    }
}
