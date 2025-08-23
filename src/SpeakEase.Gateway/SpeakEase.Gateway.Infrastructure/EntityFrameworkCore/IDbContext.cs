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
    DbSet<SysUserEntity> SysUser { get; set; }

    /// <summary>
    /// 系统日志
    /// </summary>
    DbSet<SysLogEntity> SysLog { get; set; }
    
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

    /// <summary>
    /// 重写SaveChangesAsync方法，在保存更改前执行额外操作
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 重写SaveChangesAsync方法，在保存更改前执行额外操作
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

    /// <summary>
    /// 重写SaveChanges方法，在保存更改前执行额外操作
    /// </summary>
    /// <returns></returns>
    int SaveChanges();

    /// <summary>
    /// 重写SaveChanges方法，在保存更改前执行额外操作
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <returns></returns>
    int SaveChanges(bool acceptAllChangesOnSuccess);
}