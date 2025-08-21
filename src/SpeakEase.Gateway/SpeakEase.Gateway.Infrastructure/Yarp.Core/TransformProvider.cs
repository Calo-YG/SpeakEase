using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SpeakEase.Gateway.Domain.Const;
using SpeakEase.Gateway.Domain.Entity.Gateway;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using Yarp.ReverseProxy.Transforms;
using Microsoft.EntityFrameworkCore;

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
        var currentEndpoint = transformContext.HttpContext.GetEndpoint();

        if(currentEndpoint == null)
        {
            throw new Exception("Endpoint is null");
        }

        var displayName = currentEndpoint.DisplayName;

        if(string.IsNullOrEmpty(displayName))
        {
            throw new Exception("DisplayName is null");
        }

        var target = await GetTargetRouteAsync(displayName);

        // 获取路由匹配时捕获的 {catch-all} 参数值
        if (transformContext.HttpContext.Request.RouteValues.TryGetValue("catch-all", out var catchAllValue) ||
            transformContext.HttpContext.Request.RouteValues.TryGetValue("remainder", out catchAllValue))
        {
            var catchAllPath = catchAllValue?.ToString();
        
            // 构建新的路径，将捕获的路径片段拼接到 /api/ 后面
            var newPath = $"/{target}/{catchAllPath}";
        
            // 安全地更新路径
            transformContext.Path = new PathString(newPath);
        }
    }

    /// <summary>
    /// 获取目标路由
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    private async Task<string> GetTargetRouteAsync(string code)
    {
        var key = string.Format(CacheConst.TargeRouteCacheKey, code);

        var tryRoute = memoryCache.TryGetValue<string>(key,out var route);

        if (tryRoute)
        {
            return route!;
        }

        route = await context.QueryNoTracking<RouterEntity>().Where(x => x.AppName == code).Select(p=>p.TargetRoute).FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(route))
        {
            throw new Exception("TargetRoute is null");
        }

        memoryCache.Set(key, route, new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30),
            SlidingExpiration = TimeSpan.FromMinutes(10)
        });

        return route;
    }
}