namespace SpeakEase.Gateway.Infrastructure.GatewayLog
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogOperate
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Module { get; }

        /// <summary>
        /// 路由名称
        /// </summary>
        public string RouteName { get; }

        /// <summary>
        /// 写入数据库
        /// </summary>
        public bool WriteDatabase { get; }
    }
}
