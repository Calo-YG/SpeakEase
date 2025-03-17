using Microsoft.EntityFrameworkCore;

namespace SpeakEase.Application;

public abstract class ApplicationBase<TDbContext>(IServiceProvider serviceProvider, TDbContext dbContext)
    where TDbContext : DbContext
{
}