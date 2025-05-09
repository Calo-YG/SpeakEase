namespace SpeakEase.Infrastructure.Options
{
    public class RedisOptions
    {
        /// <summary>
        /// 无密码连接字符串
        /// </summary>
        private readonly string NoPassWordConnectionString = "{0}:{1};defaultDatabase={2};max poolsize={3};min poolsize={4};connectTimeout={5};retry={6};prefix={7}";

        /// <summary>
        /// 有密码连接字符串
        /// </summary>
        private readonly string Connection = "{0}:{1};user={2};password={3};defaultDatabase={4};max poolsize={5};min poolsize={6};connectTimeout={7};retry={8};prefix={9}";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get => BuildConnectionString(); }

        /// <summary>
        /// 用户名
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;    

        /// <summary>
        /// 默认数据库
        /// </summary>
        public int DefaultDatabase { get; set; }

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxPoolSize { get; set; } = 100;

        /// <summary>
        /// 最小连接数
        /// </summary>
        public int MinPoolSize { get; set; } = 5;

        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int ConnectTimeout { get; set; } = 5000;

        /// <summary>
        /// 连接重试次数
        /// </summary>
        public int Retry { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }

        private string BuildConnectionString()
        {
            if (string.IsNullOrEmpty(Password))
            {
                return string.Format(NoPassWordConnectionString, Host, Port,DefaultDatabase, MaxPoolSize, MinPoolSize, ConnectTimeout, Retry, Prefix);
            }

            return string.Format(Connection,Host,Port,User,Password,DefaultDatabase,MaxPoolSize,MinPoolSize,ConnectTimeout,Retry,Prefix);
        }
    }

}
