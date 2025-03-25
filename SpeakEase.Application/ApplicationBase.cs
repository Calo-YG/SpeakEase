using Microsoft.EntityFrameworkCore;

namespace SpeakEase.Application;

/// <summary>
/// 基类
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <param name="serviceProvider"></param>
/// <param name="dbContext"></param>
public abstract class ApplicationBase<TDbContext>(IServiceProvider serviceProvider, TDbContext dbContext)
    where TDbContext : DbContext
{

}