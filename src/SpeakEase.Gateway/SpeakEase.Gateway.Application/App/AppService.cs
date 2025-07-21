using Microsoft.EntityFrameworkCore;
using SpeakEase.Gateway.Contract.App;
using SpeakEase.Gateway.Contract.App.Dto;
using SpeakEase.Gateway.Domain.Entity.Gateway;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Shared;
using SpeakEase.Infrastructure.SpeakEase.Core;
using SpeakEase.Infrastructure.WorkIdGenerate;
using Yitter.IdGenerator;

namespace SpeakEase.Gateway.Application.App;

/// <summary>
/// 应用服务
/// </summary>
public class AppService(IDbContext context,IIdGenerate idGenerate):IAppService
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
        
        var id = idGenerate.NewIdString();
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
        
        return await query
            .Select(p => new AppPageDto
            {
                Id = p.Id,
                AppName = p.AppName,
                AppKey = p.AppKey,
                AppCode = p.AppCode,
                AppDescription = p.AppDescription,
                CreatedAt = p.CreatedAt
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToPageResultAsync(input.Pagination);
    }
    
    /// <summary>
    /// 获取应用详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<AppDto> GetAppById(string id)
    { 
        return context.QueryNoTracking<AppEntity>()
            .Where(p=>p.Id == id)
            .Select(p => new AppDto
            {
                AppName = p.AppName,
                AppKey = p.AppKey,
                AppCode = p.AppCode,
                AppDescription = p.AppDescription
            })
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// 修改应用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAppAsync(UpdateAppInput input)
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
        
        var entity = context.App.FirstOrDefault(x => x.Id == input.Id);

        if (entity is null)
        {
            throw new UserFriendlyException("应用不存在");
        }
        
        entity.Update(input.AppName, input.AppKey, input.AppCode, input.AppDescription);
        
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// 获取下拉框列表
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<List<AppSelectDto>> GetSelectAsync(string name)
    {
        return  context.QueryNoTracking<AppEntity>()
            .WhereIf(string.IsNullOrEmpty( name),p=>p.AppName.Contains( name) || p.AppCode.Contains( name))
            .Select(x => new AppSelectDto
           {
               Id = x.Id,
               AppName = x.AppName,
               AppCode = x.AppCode
            }).ToListAsync();
    }
}