using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using Yarp.ReverseProxy.Transforms;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core;

/// <summary>
/// 配置转换
/// </summary>
public class TransformProvider(IDbContext context,IMemoryCache memoryCache): ITransformProvider
{
    /// <summary>
    /// 数据库转换
    /// </summary>
    /// <param name="transformContext"></param>
    /// <returns></returns>
    public async Task DatabaseTransformAsync(RequestTransformContext transformContext)
    {
        // 获取路由匹配时捕获的 {catch-all} 参数值
        if (transformContext.HttpContext.Request.RouteValues.TryGetValue("catch-all", out var catchAllValue) ||
            transformContext.HttpContext.Request.RouteValues.TryGetValue("remainder", out catchAllValue))
        {
            var catchAllPath = catchAllValue?.ToString();
        
            // 构建新的路径，将捕获的路径片段拼接到 /api/ 后面
            var newPath = $"/api/{catchAllPath}";
        
            // 安全地更新路径
            transformContext.Path = new PathString(newPath);
        }
        
        await Task.CompletedTask;
    }
}