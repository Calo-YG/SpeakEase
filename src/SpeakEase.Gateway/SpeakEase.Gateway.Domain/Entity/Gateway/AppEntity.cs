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
        public string AppName { get;private set; }
        
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppKey { get;private set; }

        /// <summary>
        /// 应用编码
        /// </summary>
        public string AppCode { get;private set; }
        
        /// <summary>
        /// 应用描述
        /// </summary>
        public string AppDescription { get;private set; }
        
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyDate { get;private set; }
        
        /// <summary>
        /// 申请时间
        /// </summary>
        public string ApplyBy { get;private set; }
        
        protected AppEntity()
        {
            
        }
        
        /// <summary>
        /// 创建应用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appName"></param>
        /// <param name="appKey"></param>
        /// <param name="appCode"></param>
        /// <param name="appDescription"></param>
        /// <param name="applyDate"></param>
        /// <param name="applyBy"></param>
        public AppEntity(string id,string appName, string appKey, string appCode, string appDescription, DateTime applyDate, string applyBy="system")
        {
            Id = id;
            AppName = appName;
            AppKey = appKey;
            AppCode = appCode;
            AppDescription = appDescription;
            ApplyDate = applyDate;
            ApplyBy = applyBy;
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appKey"></param>
        /// <param name="appCode"></param>
        /// <param name="appDescription"></param>
        public void Update(string appName, string appKey, string appCode, string appDescription)
        { 
            AppName = appName;
            AppKey = appKey;
            AppCode = appCode;
            AppDescription = appDescription;
        }
    }
}
