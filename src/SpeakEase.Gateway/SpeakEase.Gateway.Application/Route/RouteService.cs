using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SpeakEase.Gateway.Contract.Route;
using SpeakEase.Gateway.Contract.Route.Dto;
using SpeakEase.Gateway.Domain.Entity.Gateway;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Shared;
using SpeakEase.Infrastructure.WorkIdGenerate;

namespace SpeakEase.Gateway.Application.Route;

/// <summary>
/// 路由服务
/// </summary>
public class RouteService(IDbContext  context,IIdGenerate idGenerate): IRouteService
{
    /// <summary>
    /// 创建路由
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task CreateRouteAsync(CreateRouteInput input)
    {
        if (string.IsNullOrEmpty(input.AppId) || string.IsNullOrEmpty(input.AppName))
        {
            throw new UserFriendlyException("请选择应用");
        }

        if (string.IsNullOrEmpty(input.Prefix))
        {
            throw new UserFriendlyException("请输入路由前缀");
        }
        
        var id = idGenerate.NewIdString();
        
        var entity = new RouterEntity(id,
            input.AppId, 
            input.AppName, 
            input.Prefix, 
            input.Sort, 
            input.AuthorizationPolicy, 
            input.RateLimiterPolicy, 
            input.OutputCachePolicy, 
            input.TimeoutPolicy, 
            input.Timeout, 
            input.CorsPolicy, 
            input.MaxRequestBodySize,
            input.TargetRoute);
        
        entity.SetClusterId(idGenerate.NewIdString());

        await context.Router.AddAsync(entity);
        
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// 更新路由
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task UpdateRouteAsync(UpdateRouteInput input)
    {
        if (string.IsNullOrEmpty(input.Prefix))
        {
            throw new UserFriendlyException("请输入路由前缀");
        }
        
        var entity = context.Router.FirstOrDefault(x => x.Id == input.Id);

        if (entity is null)
        {
            throw new UnreachableException("路由不存在");
        }
        
        entity.Update(input.Prefix, input.Sort, input.AuthorizationPolicy, input.RateLimiterPolicy, input.OutputCachePolicy, input.TimeoutPolicy, input.Timeout, input.CorsPolicy, input.MaxRequestBodySize,input.TargeRoute);
        
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// 路由分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<PageResult<RoutePageDto>> GetListAsync(RoutePageInput input)
    {
        var query = context.QueryNoTracking<RouterEntity>()
            .WhereIf(!string.IsNullOrEmpty(input.AppId), x => x.AppId == input.AppId)
            .WhereIf(!string.IsNullOrEmpty(input.AppName), x => x.AppName.Contains(input.AppName))
            .WhereIf(input.StartTime != null, x => x.CreatedAt >= input.StartTime)
            .WhereIf(input.EndTime != null, x => x.CreatedAt <= input.EndTime)
            .OrderByDescending(x => x.CreatedAt);

        return query.Select(p => new RoutePageDto
        {
            Id = p.Id,
            AppId = p.AppId,
            AppName = p.AppName,
            Prefix = p.Prefix,
            Sort = p.Sort,
            ClusterId = p.ClusterId,
            AuthorizationPolicy = p.AuthorizationPolicy,
            RateLimiterPolicy = p.RateLimiterPolicy,
            OutputCachePolicy = p.OutputCachePolicy,
            TimeoutPolicy = p.TimeoutPolicy,
            Timeout = p.Timeout,
            CorsPolicy = p.CorsPolicy,
            MaxRequestBodySize = p.MaxRequestBodySize,
            CreatedAt = p.CreatedAt,
            ModifyAt = p.ModifyAt
        }).ToPageResultAsync(input.Pagination);
    }

    /// <summary>
    /// 获取路由详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<RouteDto> GetByIdAsync(string id)
    {
        return context.QueryNoTracking<RouterEntity>()
            .Where(p => p.Id == id)
            .Select(p => new RouteDto
            {
                Id = p.Id,
                AppId = p.AppId,
                AppName = p.AppName,
                Prefix = p.Prefix,
                Sort = p.Sort,
                ClusterId = p.ClusterId,
                AuthorizationPolicy = p.AuthorizationPolicy,
                RateLimiterPolicy = p.RateLimiterPolicy,
                OutputCachePolicy = p.OutputCachePolicy,
                TimeoutPolicy = p.TimeoutPolicy,
                Timeout = p.Timeout,
                CorsPolicy = p.CorsPolicy,
                MaxRequestBodySize = p.MaxRequestBodySize
            }).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 删除路由
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task DeleteAsync(string id)
    {
        var any = context.Router.Any(p => p.Id == id);

        if (!any)
        {
            throw new UserFriendlyException("路由不存在");
        }

        await context.Router.Where(p=>p.Id == id).ExecuteDeleteAsync();
        
        await context.SaveChangesAsync();
    }
}