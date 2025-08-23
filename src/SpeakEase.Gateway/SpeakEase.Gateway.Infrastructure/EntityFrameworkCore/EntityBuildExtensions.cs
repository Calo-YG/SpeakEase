using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpeakEase.Domain.Contract.Building.Block.Domain;
using SpeakEase.Gateway.Domain.Entity.Gateway;
using SpeakEase.Gateway.Domain.Entity.System;
using SpeakEase.Gateway.Domain.Enum;

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
            op.Property(p => p.CreatedAt).HasColumnType("timestamp with time zone");
            op.Property(p => p.CreateByName).HasMaxLength(50);
            op.Property(p=>p.CreateById).HasMaxLength(50);
        });

        return builder;
    }

    private static ModelBuilder BuilderModify<T>(this ModelBuilder builder) where T : class, IModify
    {
        builder.Entity<T>(op =>
        {
            op.Property(p => p.ModifyAt).HasColumnType("timestamp with time zone");
            op.Property(p => p.ModifyByName).HasMaxLength(50);
            op.Property(p => p.ModifyById).HasMaxLength(50);
        });

        return builder;
    }
    
    private static ModelBuilder BuilderDeleted<T>(this ModelBuilder builder) where T : class, IDeleted
    {
        builder.Entity<T>(op =>
        {
            op.Property(p => p.IsDeleted);
            op.Property(p => p.DeleteAt).HasColumnType("timestamp with time zone");
            op.Property(p => p.DeletedByName).HasMaxLength(50);
            op.Property(p => p.DeletedById).HasMaxLength(50);
        });

        return builder;
    }
    
    public static ModelBuilder Builder(this ModelBuilder builder) 
    {
        #region system

        builder.Entity<SysUserEntity>(op =>
        {
            op.ToTable("sys_user");
            op.HasKey(p => p.Id);
            op.Property(p => p.Id).HasMaxLength(50);
            op.Property(p => p.Account).HasMaxLength(50).IsRequired();
            op.Property(p => p.Name).HasMaxLength(20).IsRequired();
            op.Property(p => p.Email).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.Avatar).HasMaxLength(255).IsRequired(false);
        })
        .BuilderCration<SysUserEntity>()
        .BuilderModify<SysUserEntity>();

        builder.Entity<SysLogEntity>(op =>
        {
            op.ToTable("sys_log");
            op.HasKey(p => p.Id);
            op.Property(p => p.Id).HasMaxLength(50);
            op.Property(p=>p.IpAddress).HasMaxLength(50).IsRequired(false);
            op.Property(p=>p.Module).HasMaxLength(50).IsRequired();
            op.Property(p=>p.Message).HasMaxLength(255).IsRequired(false);
            op.Property(p=>p.Route).HasMaxLength(255).IsRequired();
            op.Property(p=>p.Trace).HasMaxLength(1024).IsRequired(false);
            op.Property(p=>p.RequestData).HasMaxLength(1024).IsRequired(false);
            op.Property(p=>p.Agent).HasMaxLength(100).IsRequired(false);
            op.Property(p => p.LogLevel).HasConversion<EnumToNumberConverter<LogLevelEnum, int>>();
            op.Property(p => p.LogType).HasConversion<EnumToNumberConverter<LogTypeEnum, int>>();
        });

        #endregion

        #region Gateway

        builder.Entity<AppEntity>(op =>
        {
            op.ToTable("app");
            op.HasKey(p => p.Id);
            op.Property(p => p.Id).HasMaxLength(50);
            op.Property(p => p.AppName).HasMaxLength(50).IsRequired();
            op.Property(p => p.AppKey).HasMaxLength(50).IsRequired();
            op.Property(p => p.AppCode).HasMaxLength(50).IsRequired();
            op.Property(p => p.AppDescription).HasMaxLength(255).IsRequired(false);
        })
        .BuilderCration<AppEntity>()
        .BuilderModify<AppEntity>();

        builder.Entity<RouterEntity>(op =>
        {
            op.ToTable("route");
            op.HasKey(p => p.Id);
            op.HasIndex(p => p.ClusterId);
            op.Property(p => p.Id).HasMaxLength(50);
            op.Property(p => p.AppId).HasMaxLength(50).IsRequired();
            op.Property(p => p.AppName).HasMaxLength(50).IsRequired();
            op.Property(p => p.Prefix).HasMaxLength(50).IsRequired();
            op.Property(p=>p.TargetRoute).HasMaxLength(255).IsRequired();
            op.Property(p => p.ClusterId).HasMaxLength(50).IsRequired();
            op.Property(p => p.AuthorizationPolicy).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.RateLimiterPolicy).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.OutputCachePolicy).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.TimeoutPolicy).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.CorsPolicy).HasMaxLength(50).IsRequired(false);
            op.Property(p => p.MaxRequestBodySize).HasMaxLength(50).IsRequired(false);
            op.Property(p=>p.TargetRoute).HasMaxLength(125).IsRequired();
        })
        .BuilderCration<RouterEntity>()
        .BuilderModify<RouterEntity>();

        builder.Entity<ClusterEntity>(op =>
            {
                op.ToTable("cluster");
                op.HasKey(p => p.Id);
                op.HasIndex(p => p.ClusterId);
                op.Property(p => p.Id).HasMaxLength(50);
                op.Property(p => p.ClusterId).HasMaxLength(50).IsRequired();
                op.Property(p => p.LoadBalance).HasMaxLength(50).IsRequired();
                op.Property(p => p.Policy).HasMaxLength(50).IsRequired();
                op.Property(p => p.Path).HasMaxLength(50).IsRequired();
                op.Property(p => p.AvailableDestinationsPolicy).HasMaxLength(50).IsRequired(false);
                op.Property(p => p.Address).HasMaxLength(2550).IsRequired(false);
            }
        )
        .BuilderCration<ClusterEntity>()
        .BuilderModify<ClusterEntity>();

        builder.Entity<DestinationEntity>(op =>
        {
            op.ToTable("destination");
            op.HasKey(p => p.Id);
            op.Property(p => p.Id).HasMaxLength(50);
            op.Property(p => p.Address).HasMaxLength(255).IsRequired();
            op.Property(p => p.State).HasConversion<EnumToNumberConverter<DestinationStateEnum,int>>();
        })
        .BuilderCration<DestinationEntity>()
        .BuilderModify<DestinationEntity>();
        #endregion
        return builder;
    }
}