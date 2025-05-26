namespace SpeakEase.BuildingBlock.Grpc.BuildingBlock.Grpc.Options
{
    public class GrpcServerOptions
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int Timeout { get; set; } = 30;
        /// <summary>
        /// 是否启用压缩
        /// </summary>
        public bool EnableCompression { get; set; }
        /// <summary>
        /// 是否启用日志
        /// </summary>
        public bool EnableLog { get; set; }

        /// <summary>
        /// DNS 服务器 IP 地址   
        /// </summary>
        public string DnsServerIp { get; set; }

        /// <summary>
        /// GRPC 服务器 名称
        /// </summary>
        public string ServiceName { get; set; }
    }
}
