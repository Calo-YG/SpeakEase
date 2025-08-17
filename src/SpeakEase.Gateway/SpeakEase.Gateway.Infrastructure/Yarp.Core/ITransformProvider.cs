using Yarp.ReverseProxy.Transforms;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core;

/// <summary>
/// 配置转换
/// </summary>
public interface ITransformProvider
{
    Task DatabaseTransformAsync(RequestTransformContext transformContext);
}