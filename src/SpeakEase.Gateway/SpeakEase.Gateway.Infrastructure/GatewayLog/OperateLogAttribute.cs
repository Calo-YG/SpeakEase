namespace SpeakEase.Gateway.Infrastructure.GatewayLog
{
    public class OperateLogAttribute : Attribute, ILogOperate
    {
        public string Module { get; private set; }

        public string RouteName { get; private set; }

        public bool WriteDatabase { get; private set; }

        public OperateLogAttribute(string module, string routeName,bool writeDatabase = false)
        {
            Module = module;
            RouteName = routeName;
            WriteDatabase = writeDatabase;
        }
    }
}
