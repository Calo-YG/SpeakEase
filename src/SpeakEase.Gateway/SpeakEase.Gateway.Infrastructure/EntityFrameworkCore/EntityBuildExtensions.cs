using Microsoft.EntityFrameworkCore;
using SpeakEase.Domain.Contract.Building.Block.Domain;
using SpeakEase.Gateway.Domain.Entity.Gateway;
using SpeakEase.Gateway.Domain.Entity.System;

namespace SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;

/// <summary>
/// 构建实体
/// </summary>
public static class EntityBuildExtensions
{
    private static ModelBuilder BuilderCration<T>(this ModelBuilder builder) where T : class,ICreation
    {
        builder.Entity<T>(op =>
        {
            op.Property(p => p.CreateByName).HasMaxLength(50);
            op.Property(p=>p.CreateById).HasMaxLength(50);
        });

        return builder;
    }

    private static ModelBuilder BuilderModify<T>(this ModelBuilder builder) where T : class, IModify
    {
        builder.Entity<T>(op =>
        {
            op.Property(p => p.ModifyByName).HasMaxLength(50);
            op.Property(p => p.ModifyById).HasMaxLength(50);
        });

        return builder;
    }
    
    private static ModelBuilder BuilderDeleted<T>(this ModelBuilder builder) where T : class, IDeleted
    {
        builder.Entity<T>(op =>
        {
            op.Property(p => p.DeletedByName).HasMaxLength(50);
            op.Property(p => p.DeletedById).HasMaxLength(50);
        });

        return builder;
    }
    
    public static ModelBuilder Builder(this ModelBuilder builder) 
    {
        #region system

        builder.Entity<SysUser>(op =>
        {
            op.HasKey(p => p.Id);
            op.Property(p => p.Id).HasMaxLength(50);
            op.Property(p => p.Account).HasMaxLength(50).IsRequired();
            op.Property(p => p.Name).HasMaxLength(20).IsRequired();
            op.Property(p => p.Email).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.Avatar).HasMaxLength(255).IsRequired(false);
        }).BuilderCration<SysUser>().BuilderModify<SysUser>();

        #endregion

        #region Gateway

        builder.Entity<AppEntity>(op =>
        {
            op.HasKey(p => p.Id);
            op.Property(p => p.Id).HasMaxLength(50);
            op.Property(p => p.AppName).HasMaxLength(50).IsRequired();
            op.Property(p => p.AppKey).HasMaxLength(50).IsRequired();
            op.Property(p => p.AppCode).HasMaxLength(50).IsRequired();
            op.Property(p => p.AppDescription).HasMaxLength(255).IsRequired(false);
        }).BuilderCration<AppEntity>().BuilderModify<AppEntity>();

        builder.Entity<RouterEntity>(op =>
        { 
            op.HasKey(p => p.Id);
            op.Property(p => p.Id).HasMaxLength(50);
            op.Property(p => p.AppId).HasMaxLength(50).IsRequired();
            op.Property(p => p.AppName).HasMaxLength(50).IsRequired();
            op.Property(p => p.Prefix).HasMaxLength(50).IsRequired();
            op.Property(p => p.ClusterId).HasMaxLength(50).IsRequired();
            op.Property(p => p.AuthorizationPolicy).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.RateLimiterPolicy).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.OutputCachePolicy).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.TimeoutPolicy).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.CorsPolicy).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.MaxRequestBodySize).HasMaxLength(50).IsRequired(false);
        }).BuilderCration<RouterEntity>().BuilderModify<RouterEntity>();

        builder.Entity<ClusterEntity>(op =>
            {
                op.HasKey(p => p.Id);
                op.Property(p => p.Id).HasMaxLength(50);
                op.Property(p => p.ClusterId).HasMaxLength(50).IsRequired();
                op.Property(p => p.LoadBalance).HasMaxLength(50).IsRequired();
                op.Property(p => p.Policy).HasMaxLength(50).IsRequired();
                op.Property(p => p.Path).HasMaxLength(50).IsRequired();
            }
        ).BuilderCration<ClusterEntity>().BuilderModify<ClusterEntity>();
        #endregion
        return builder;
    }
}