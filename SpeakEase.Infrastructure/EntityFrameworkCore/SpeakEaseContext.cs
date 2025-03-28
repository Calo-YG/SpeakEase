using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpeakEase.Domain.Shared;
using SpeakEase.Domain.Users;
using SpeakEase.Domain.Users.Enum;
using SpeakEase.Infrastructure.Authorization;

namespace SpeakEase.Infrastructure.EntityFrameworkCore;

/// <summary>
/// 数据库上下文
/// </summary>
/// <param name="configuration"></param>
/// <param name="userContext"></param>
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

    private readonly IUserContext _userContext;

    public SpeakEaseContext(DbContextOptions<SpeakEaseContext> options, IUserContext userContext) : base(options)
    {
        this._userContext = userContext;
    }

    public SpeakEaseContext(DbContextOptions<SpeakEaseContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        var converter = new ValueConverter<SourceEnum, int>(
            v => ((int)v),
            v => (SourceEnum)v);

        modelBuilder.Entity<UserEntity>(op =>
        {
            op.HasKey(p => p.Id);
            op.Property(p=>p.UserPassword).IsRequired().HasMaxLength(128);
            op.Property(p => p.UserAccount).IsRequired().HasMaxLength(50);
            op.Property(p => p.Source).HasConversion(converter);
        });

        modelBuilder.Entity<RefreshTokenEntity>(op => 
        {
            op.HasKey(p => p.Id);
            op.Property(p => p.Token).IsRequired();
            op.Property(p=>p.UserId).IsRequired();
            op.Property(p => p.IsUsed).IsRequired().HasDefaultValue(false);    
            op.Property(p => p.Expires).IsRequired();
        });

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
                creation.UserId = _userContext.UserId;
                creation.UserName = _userContext.UserName;
            }else if(item.State == EntityState.Modified && item.Entity is IModify modify)
            {
                modify.ModifyUserId = _userContext.UserId;
                modify.ModifyUserName = _userContext.UserName;
                modify.ModifyAt = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// 迁移
    /// </summary>
    public void Migrate()
    {
        this.Database.Migrate();
    }
}