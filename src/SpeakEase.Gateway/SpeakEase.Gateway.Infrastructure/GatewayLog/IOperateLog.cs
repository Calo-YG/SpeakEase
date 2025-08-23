namespace SpeakEase.Gateway.Infrastructure.GatewayLog
{
    internal interface IOperateLog
    {
       public Task WriteLogAsync(OperateLog operate);
    }
}
