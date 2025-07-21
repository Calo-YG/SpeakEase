using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Gateway.Infrastructure.Yarp.Core.BuildBlock;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core.Contrib;

/// <summary>
/// 路由解析
/// </summary>
/// <param name="context">数据库上下文</param>
/// <param name="httpContextAccessor">http请求上下文</param>
/// <param name="memoryCache">内存缓存</param>
public class RouteResolve(IDbContext context,IHttpContextAccessor httpContextAccessor,IMemoryCache memoryCache):IRouteResolve
{
    /// <summary>
    /// 路由解析
    /// </summary>
    /// <returns></returns>
    private string GetServiceCodeFromHeader()
    {
        return httpContextAccessor!.HttpContext!.Request.Headers["ServiceCode"].ToString();
    }
    
    /// <summary>
    /// 获取服务码
    /// </summary>
    /// <returns></returns>
    private string GetServiceCodeFromQuery()
    {
        return httpContextAccessor!.HttpContext!.Request.Query["ServiceCode"].ToString();
    }
    
    
}