using Microsoft.EntityFrameworkCore;
using SpeakEase.Gateway.Contract.Cluster;
using SpeakEase.Gateway.Contract.Cluster.Dto;
using SpeakEase.Gateway.Domain.Entity.Gateway;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.SpeakEase.Core;
using SpeakEase.Infrastructure.WorkIdGenerate;

namespace SpeakEase.Gateway.Application.Cluster;

/// <summary>
/// 集群配置服务
/// </summary>
public class ClusterService(IDbContext context, IIdGenerate idGenerate):IClusterService
{
    /// <summary>
    /// 创建集群
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task CreateClusterAsync(CreateClusterInput input)
    {
        if (string.IsNullOrEmpty(input.ClusterId))
        {
            throw new UserFriendlyException("请输入集群Id");
        }
        
        if (string.IsNullOrEmpty(input.LoadBalance))
        {
            throw new UserFriendlyException("请选择负载均衡算法");
        }

        if (input.Enabled && string.IsNullOrEmpty(input.Policy) || string.IsNullOrEmpty(input.Path))
        {
            throw new UserFriendlyException("请输入节点健康策略和路径");
        }

        if (string.IsNullOrEmpty(input.Address))
        {
            throw new UserFriendlyException("请输入节点地址");
        }

        var address = input.Address.Split(',').Select(StringExtensions.EnsureHttpPrefix);

        var id = idGenerate.NewIdString();
        var entity = new ClusterEntity(id,
            input.ClusterId, 
            input.LoadBalance, 
            input.Enabled, 
            input.Interval, 
            input.Timeout,
            input.Policy, 
            input.Path, 
            input.AvailableDestinationsPolicy);

        entity.SetAddress(string.Join(",", address));
        
        await context.Cluster.AddAsync(entity);
        
        await context.SaveChangesAsync();
    }
    
    /// <summary>
    /// 更新集群
    /// </summary>
    /// <param name="input"></param>
    public async Task UpdateClusterAsync(UpdateClusterInput input)
    { 
        if (string.IsNullOrEmpty(input.ClusterId))
        {
            throw new UserFriendlyException("请输入集群Id");
        }
        
        if (string.IsNullOrEmpty(input.LoadBalance))
        {
            throw new UserFriendlyException("请选择负载均衡算法");
        }

        if (input.Enabled && string.IsNullOrEmpty(input.Policy) || string.IsNullOrEmpty(input.Path))
        {
            throw new UserFriendlyException("请输入节点健康策略和路径");
        }
        
        if (string.IsNullOrEmpty(input.Address))
        {
            throw new UserFriendlyException("请输入节点地址");
        }

        var address = input.Address.Split(',').Select(StringExtensions.EnsureHttpPrefix);
        
        var entity = await context.Cluster.FirstOrDefaultAsync(x => x.Id == input.Id);
        
        if (entity == null)
        {
            throw new UserFriendlyException("集群不存在");
        }
        
        entity.Update(input.LoadBalance, input.Enabled, input.Interval, input.Timeout, input.Policy, input.Path, input.AvailableDestinationsPolicy);
        
        entity.SetAddress(string.Join(",", address));
        
        await context.SaveChangesAsync();
    }
    
    /// <summary>
    /// 删除集群
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task DeleteClusterAsync(string id)
    {
        var entity = await context.Cluster.FirstOrDefaultAsync(x => x.Id == id);
        
        if (entity == null)
        {
            throw new UserFriendlyException("集群不存在");
        }

        await context.Cluster.Where(p => p.Id == id).ExecuteDeleteAsync();
        
        await context.SaveChangesAsync();
    }
    
    /// <summary>
    /// 获取汲取信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task<ClusterDto> GetByIdAsync(string id)
    {
        var entity = await context.Cluster.FirstOrDefaultAsync(x => x.Id == id);
        
        if (entity == null)
        {
            throw new UserFriendlyException("集群不存在");
        }
        
        var dto = new ClusterDto
        {
            Id = entity.Id,
            ClusterId = entity.ClusterId,
            LoadBalance = entity.LoadBalance,
            Enabled = entity.Enabled,
            Interval = entity.Interval,
            Timeout = entity.Timeout,
            Policy = entity.Policy,
            Path = entity.Path,
            AvailableDestinationsPolicy = entity.AvailableDestinationsPolicy
        };

        return dto;
    }
}