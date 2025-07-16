using Microsoft.EntityFrameworkCore;
using SpeakEase.Gateway.Contract.App;
using SpeakEase.Gateway.Contract.App.Dto;
using SpeakEase.Gateway.Domain.Entity.Gateway;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Shared;
using SpeakEase.Infrastructure.SpeakEase.Core;
using Yitter.IdGenerator;

namespace SpeakEase.Gateway.Application.App;

/// <summary>
/// 应用服务
/// </summary>
public class AppService(IDbContext context):IAppService
{
    /// <summary>
    /// 创建应用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task CreateAppAsync(CreateAppInput input)
    {
        if (string.IsNullOrEmpty(input.AppName))
        {
            throw new UserFriendlyException("请输入应用名称");
        }

        if (string.IsNullOrEmpty(input.AppKey))
        {
            throw new UserFriendlyException("请输入应用密钥");
        }
        
        if (string.IsNullOrEmpty(input.AppCode))
        {
            throw new UserFriendlyException("请输入应用编码");
        }
        
        var any = context.App.Any(x => x.AppKey == input.AppKey && x.AppCode == input.AppCode);

        if (any)
        {
            throw new UserFriendlyException("应用已存在");
        }

        var longId = YitIdHelper.NextId();
        var id = LongToStringConverter.Convert(longId);
        var entity = new AppEntity(id,input.AppName, input.AppKey, input.AppCode, input.AppDescription,DateTime.Now);

        await context.App.AddAsync(entity);
        
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// 获取应用列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PageResult<AppPageDto>> GetList(AppPageInput input)
    {
        var query = context.QueryNoTracking<AppEntity>();
        
        query = query.WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.AppName.Contains(input.Keyword) 
                                                                         || x.AppKey.Contains(input.Keyword) 
                                                                         || x.AppCode.Contains(input.Keyword));
        
        query = query.WhereIf(input.StartTime != null, x => x.CreatedAt >= input.StartTime);
        query = query.WhereIf(input.EndTime != null, x => x.CreatedAt <= input.EndTime);
        
        var count = await query.CountAsync();
        var list = await query
            .Select(p=> new AppPageDto
            {
                Id = p.Id,
                AppName = p.AppName,
                AppKey = p.AppKey,
                AppCode = p.AppCode,
                AppDescription = p.AppDescription,
                CreatedAt = p.CreatedAt 
            })
            .OrderByDescending(x => x.CreatedAt)
            .Skip((input.Pagination.Page - 1) * input.Pagination.PageSize)
            .Take(input.Pagination.PageSize).ToListAsync();

        return PageResult<AppPageDto>.Create(count, list);
    }
}