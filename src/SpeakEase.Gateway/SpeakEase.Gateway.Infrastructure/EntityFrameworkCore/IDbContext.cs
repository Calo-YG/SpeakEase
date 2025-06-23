using SpeakEase.Authorization.Authorization;

namespace SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;

public interface IDbContext
{
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