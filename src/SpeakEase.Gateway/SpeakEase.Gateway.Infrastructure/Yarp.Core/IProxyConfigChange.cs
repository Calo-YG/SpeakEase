namespace SpeakEase.Gateway.Infrastructure.Yarp.Core;

/// <summary>
/// 配置变更
/// </summary>
public interface IProxyConfigChange
{
    /// <summary>
    /// 刷新
    /// </summary>
    void Refresh();
    /// <summary>
    /// 异步刷新
    /// </summary>
    /// <returns></returns>
    Task RefreshAsync();
}