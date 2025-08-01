﻿using Microsoft.EntityFrameworkCore;
using SpeakEase.Authorization.Authorization;
using SpeakEase.Study.Domain.Shared;
using SpeakEase.Study.Domain.Users;
using SpeakEase.Study.Infrastructure.EntityFrameworkCore.ModelBuilders;

namespace SpeakEase.Study.Infrastructure.EntityFrameworkCore;

/// <summary>
/// 数据库上下文
/// </summary>
public class SpeakEaseContext:DbContext,IDbContext
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public DbSet<UserEntity> User { get; set; }

    /// <summary>
    /// token 刷新实体
    /// </summary>
    public DbSet<RefreshTokenEntity> RefreshToken { get; set; }

    /// <summary>
    /// 用户设置
    /// </summary>
    public DbSet<UserSettingEntity> UserSetting { get; set; }


    private readonly IUserContext _userContext;

    public SpeakEaseContext(DbContextOptions<SpeakEaseContext> options, IUserContext userContext) : base(options)
    {
        _userContext = userContext;
    }

    public SpeakEaseContext(DbContextOptions<SpeakEaseContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ConfigureModelUser();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        BeforeSaveChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        BeforeSaveChanges();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        BeforeSaveChanges();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        BeforeSaveChanges();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

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
    /// <returns></returns>
    public User GetUser()
    {
        return _userContext.User;
    }

    public IQueryable<T> QueryNoTracking<T>() where T : class
    {
        return Set<T>().AsNoTracking();
    }
}