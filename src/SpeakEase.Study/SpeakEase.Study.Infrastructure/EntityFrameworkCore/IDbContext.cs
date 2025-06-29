﻿using Microsoft.EntityFrameworkCore;
using SpeakEase.Authorization.Authorization;
using SpeakEase.Study.Domain.Users;

namespace SpeakEase.Study.Infrastructure.EntityFrameworkCore
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
        /// 用户设置
        /// </summary>
        public DbSet<UserSettingEntity> UserSetting { get; set; }

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

        /// <summary>
        ///  非跟踪查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> QueryNoTracking<T>() where T : class;
    }
}
