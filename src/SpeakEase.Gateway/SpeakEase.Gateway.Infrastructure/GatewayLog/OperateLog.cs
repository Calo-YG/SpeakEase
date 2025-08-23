using SpeakEase.Gateway.Domain.Enum;

namespace SpeakEase.Gateway.Infrastructure.GatewayLog
{
    internal class OperateLog
    {
        /// <summary>
        /// ip 地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        public string Message { get;  set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get;  set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        public int Duration { get;  set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevelEnum LogLevel { get;  set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public LogTypeEnum LogType { get;  set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string Trace { get;  set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public string RequestData { get;  set; }
    }
}
