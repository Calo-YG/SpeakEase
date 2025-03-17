using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SpeakEase.Domain;

public static class SpeakEaseContextExtensions
{
      public static IServiceCollection RegisterEntityFrameworkCoreContext(this IServiceCollection sevices,IConfiguration configuration)
      {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));

            sevices.AddPooledDbContextFactory<SpeakEaseContext>(options =>
            {
                  options.UseMySql(connectionString, serverVersion)
                        // The following three options help with debugging, but should
                        // be changed or removed for production.
                        .LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
            });
            sevices.AddScoped<SpeakEaseContextFactory>();
            sevices.AddScoped(sp=>sp.GetRequiredService<SpeakEaseContextFactory>().CreateDbContext());
            
            return sevices;
      }
}