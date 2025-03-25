using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Infrastructure.EntityFrameworkCore;

public static class SpeakEaseContextExtensions
{
    public static IServiceCollection RegisterEntityFrameworkCoreContext(this IServiceCollection sevices, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        sevices.AddDbContext<SpeakEaseContext>(option => option.UseNpgsql(connectionString));

        //sevices.AddScoped<SpeakEaseContextFactory>();
        //sevices.AddScoped(sp => sp.GetRequiredService<SpeakEaseContextFactory>().CreateDbContext());
        //sevices.AddPooledDbContextFactory<SpeakEaseContext>(o => o.UseNpgsql(connectionString, builder =>
        //{
            
        //}));

        return sevices;
    }
}