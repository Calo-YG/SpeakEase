using Microsoft.EntityFrameworkCore;
using SpeakEase.Domain.Users;

namespace SpeakEase.Domain;

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

    public SpeakEaseContext(DbContextOptions<SpeakEaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}