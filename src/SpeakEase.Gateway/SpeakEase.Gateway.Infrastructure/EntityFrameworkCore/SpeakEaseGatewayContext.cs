using Microsoft.EntityFrameworkCore;
using SpeakEase.Authorization.Authorization;
using SpeakEase.Domain.Contract.Building.Block.Domain;
using SpeakEase.Gateway.Domain.Entity.Gateway;
using SpeakEase.Gateway.Domain.Entity.System;

// SpeakEaseGatewayContext类继承自DbContext并实现IDbContext接口
// 它是用于管理数据库上下文的核心类
namespace SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;

public class SpeakEaseGatewayContext:DbContext,IDbContext   
{
    #region System

    /// <summary>
    /// 网关系统用户
    /// </summary>
    public DbSet<SysUser> SysUser { get; set; }

    #endregion

    #region Gateway
    
    /// <summary>
    /// 应用实体
    /// </summary>
    public DbSet<AppEntity> App { get; set; }
    
    /// <summary>
    /// 路由实体
    /// </summary>
    public DbSet<RouterEntity> Router { get; set; }
    
    /// <summary>
    /// 集群实体namespace SpeakEase.Gateway.Contract.App.Dto;
    /// </summary>
    public DbSet<ClusterEntity> Cluster { get; set; }
    #endregion
    
    // 用户上下文，用于获取当前操作用户的信息
    private readonly IUserContext _userContext;
    
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options">数据库上下文选项</param>
    /// <param name="userContext">用户上下文，用于获取当前用户信息</param>
    public SpeakEaseGatewayContext(DbContextOptions<SpeakEaseGatewayContext> options, IUserContext userContext) : base(options)
    {
        _userContext = userContext;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options">数据库上下文选项</param>
    public SpeakEaseGatewayContext(DbContextOptions<SpeakEaseGatewayContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Builder();
        base.OnModelCreating(modelBuilder);
    }

    // 重写SaveChangesAsync方法，在保存更改前执行额外操作
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        BeforeSaveChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    // 重写SaveChangesAsync方法，在保存更改前执行额外操作
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        BeforeSaveChanges();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    // 重写SaveChanges方法，在保存更改前执行额外操作
    public override int SaveChanges()
    {
        BeforeSaveChanges();
        return base.SaveChanges();
    }

    // 重写SaveChanges方法，在保存更改前执行额外操作
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        BeforeSaveChanges();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    // 在保存更改前执行的操作，主要用于设置创建和修改信息
    private void BeforeSaveChanges()
    {
        var changeTracker = ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified);

        foreach (var item in changeTracker)
        {
            if (item.State == EntityState.Added && item.Entity is ICreation creation)
            {
                creation.CreatedAt = DateTime.Now;
                creation.CreateById = _userContext.UserId;
                creation.CreateByName = _userContext.UserName;
            }
            if(item.State == EntityState.Modified && item.Entity is IModify modify)
            {
                modify.ModifyById = _userContext.UserId;
                modify.ModifyByName = _userContext.UserName;
                modify.ModifyAt = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// 迁移
    /// </summary>
    public void Migrate()
    {
        Database.Migrate();
    }

    /// <summary>
    /// 获取当前用户
    /// </summary>
    /// <returns>当前用户对象</returns>
    public User GetUser()
    {
        return _userContext.User;
    }

    /// <summary>
    /// 执行查询但不跟踪实体
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <returns>不跟踪的实体查询</returns>
    public IQueryable<T> QueryNoTracking<T>() where T : class
    {
        return Set<T>().AsNoTracking();
    }
}
