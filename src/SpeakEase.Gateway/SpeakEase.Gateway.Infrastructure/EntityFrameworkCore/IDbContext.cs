using Microsoft.EntityFrameworkCore;
using SpeakEase.Authorization.Authorization;
using SpeakEase.Gateway.Domain.Entity.Gateway;
using SpeakEase.Gateway.Domain.Entity.System;

namespace SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;

public interface IDbContext
{
    /// <summary>
    /// 网关系统用户实体
    /// </summary>
    DbSet<SysUser> SysUser { get; set; }
    
    /// <summary>
    /// 应用实体
    /// </summary>
    DbSet<AppEntity> App { get; set; }
    
    /// <summary>
    /// 路由实体
    /// </summary>
    DbSet<RouterEntity> Router { get; set; }
    
    /// <summary>
    /// 集群实体
    /// </summary>
    DbSet<ClusterEntity> Cluster { get; set; }
    /// <summary>
    /// 迁移
    /// </summary>
    void Migrate();

    User GetUser();

    /// <summary>
    ///  非跟踪查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IQueryable<T> QueryNoTracking<T>() where T : class;
}