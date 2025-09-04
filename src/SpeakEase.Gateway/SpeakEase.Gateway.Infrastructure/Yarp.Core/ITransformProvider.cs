using Yarp.ReverseProxy.Transforms;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core;

/// <summary>
/// 配置转换
/// </summary>
public interface ITransformProvider
{
    /// <summary>
    /// 从数据库中加载配置
    /// </summary>
    /// <param name="transformContext"></param>
    /// <returns></returns>
    Task DatabaseTransformAsync(RequestTransformContext transformContext);
}