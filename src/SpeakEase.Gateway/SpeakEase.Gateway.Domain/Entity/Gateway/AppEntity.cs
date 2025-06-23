using SpeakEase.Domain.Contract.Entity;

namespace SpeakEase.Gateway.Domain.Entity.Gateway
{
    /// <summary>
    /// 应用实体
    /// </summary>
    public class AppEntity:SpeakEaseEntity
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 应用编码
        /// </summary>
        public string AppCode { get; set; }
        
        /// <summary>
        /// 应用描述
        /// </summary>
        public string AppDescription { get; set; }
        
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyDate { get; set; }
        
        /// <summary>
        /// 申请时间
        /// </summary>
        public string ApplyBy { get; set; }
    }
}
