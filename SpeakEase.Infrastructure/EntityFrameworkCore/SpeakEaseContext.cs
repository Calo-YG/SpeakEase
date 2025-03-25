using Microsoft.EntityFrameworkCore;
using SpeakEase.Domain.Shared;
using SpeakEase.Domain.Users;
using SpeakEase.Infrastructure.Authorization;

namespace SpeakEase.Infrastructure.EntityFrameworkCore;

public class SpeakEaseContext : DbContext
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

    public SpeakEaseContext(DbContextOptions<SpeakEaseContext> options,IUserContext userContext)
        : base(options)
    {
        _userContext = userContext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetChangeTracker();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetChangeTracker();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        SetChangeTracker();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetChangeTracker();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private void SetChangeTracker()
    {
        var changeTracker = ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified);

        foreach (var item in changeTracker)
        {

            if(item.State == EntityState.Added && item.Entity is ICreation creation)
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
}