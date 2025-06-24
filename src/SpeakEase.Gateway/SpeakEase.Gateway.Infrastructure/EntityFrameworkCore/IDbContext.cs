using Microsoft.EntityFrameworkCore;
using SpeakEase.Authorization.Authorization;
using SpeakEase.Gateway.Domain.Entity.System;

namespace SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;

public interface IDbContext
{
    DbSet<SysUser> SysUser { get; set; }
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