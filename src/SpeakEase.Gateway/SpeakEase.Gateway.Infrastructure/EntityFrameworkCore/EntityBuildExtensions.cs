using Microsoft.EntityFrameworkCore;
using SpeakEase.Domain.Contract.Building.Block.Domain;
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

        return builder;
    }
}