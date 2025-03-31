using SpeakEase.Domain.Shared;

namespace SpeakEase.Domain.Users
{
    public class UserWordEntity:Entity<long>,ICreation
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 词典id
        /// </summary>
        public long WordId { get; set; }

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
