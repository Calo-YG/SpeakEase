using SpeakEase.Domain.Contract.Entity;
using SpeakEase.Gateway.Domain.Enum;

namespace SpeakEase.Gateway.Domain.Entity.System
{
    /// <summary>
    /// 日志
    /// </summary>
    public class SysLogEntity:Entity<string>
    {
        /// <summary>
        /// ip 地址
        /// </summary>
        public string IpAddress { get;private set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Module { get; private set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        public int Duration { get; private set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevelEnum LogLevel { get; private set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public LogTypeEnum LogType { get; private set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string Trace { get; private set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public string RequestData   { get; private set; }

        /// <summary>
        /// 来源客户端
        /// </summary>
        public string Agent { get; set; }
    }
}
