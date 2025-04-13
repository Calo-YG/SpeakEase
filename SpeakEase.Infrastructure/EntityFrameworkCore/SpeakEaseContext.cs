using Microsoft.EntityFrameworkCore;
using SpeakEase.Domain.Shared;
using SpeakEase.Domain.Users;
using SpeakEase.Domain.Word;
using SpeakEase.Infrastructure.Authorization;
using SpeakEase.Infrastructure.EntityFrameworkCore.ModelBuilders;

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

    /// <summary>
    /// 用户设置
    /// </summary>
    public DbSet<UserSettingEntity> UserSetting { get; set; }

    /// <summary>
    /// 词典表
    /// </summary>
    public DbSet<DictionaryWordEntity> DictionaryWord { get; set; }

    /// <summary>
    /// 单词示例
    /// </summary>
    public DbSet<WordExampleEntity> WordExample { get; set; }

    /// <summary>
    /// 用户词典
    /// </summary>
    public DbSet<UserWordEntity> UserWord { get; set; }


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
        modelBuilder
            .ConfigureModelDictionaryWord()
            .ConfigureModelUser()
            .ConfigureModelUserFriend();
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
        this.Database.Migrate();
    }

    /// <summary>
    /// 获取当前用户
    /// </summary>
    /// <returns></returns>
    public User GetUser()
    {
        return this._userContext.User;
    }

    public IQueryable<T> QueryNoTracking<T>() where T : class
    {
        return Set<T>().AsNoTracking();
    }
}