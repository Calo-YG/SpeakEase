using Microsoft.EntityFrameworkCore;
using SpeakEase.Domain.Users;
using SpeakEase.Infrastructure.Authorization;

namespace SpeakEase.Infrastructure.EntityFrameworkCore
{
    public interface IDbContext
    {
        /// <summary>
        /// 用户实体
        /// </summary>
        DbSet<UserEntity> User { get; set; }

        /// <summary>
        /// token 刷新实体
        /// </summary>
        public DbSet<RefreshTokenEntity> RefreshToken { get; set; }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 迁移
        /// </summary>
        void Migrate();

        User GetUser();
    }
}
